using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Pleer.Abstractions;

namespace Pleer.Models
{
    public class Playback: PlaybackBase
    {
        #region Singleton
        private static Playback playback;

        static Playback()
        {
            playback = new Playback();
        }

        private Playback()
        {
            
        }

        public static Playback Instance()
        {
            return playback;
        }
        #endregion

        private WaveOutEvent _outputDevice;
        private AudioFileReader _audioFile;
        private int _currentSong = -1;
        private bool _isRepeated = false;
        private bool _isMixed = false;

        public override void ChangeTimeLine(double timeLine)
        {
            if (_audioFile != null)
            {
                TimeSpan currentTime = _audioFile.TotalTime;

                int seconds = currentTime.Hours * 60 + currentTime.Minutes * 60 + currentTime.Seconds;

                seconds = (int)(seconds * timeLine);

                _audioFile.CurrentTime = new TimeSpan(0, 0, seconds);
            }
        }

        public override void ChangeVolume(float volume)
        {
            if (_outputDevice != null)
                _outputDevice.Volume = volume;
        }

        public override TimeSpan GetCurrentSongTime()
        {
            if (_audioFile != null)
                return _audioFile.CurrentTime;
            else
                return new TimeSpan(0, 0, 0);
        }

        public override TimeSpan GetFullSongTime()
        {
            if (_audioFile != null)
                return _audioFile.TotalTime;
            else
                return new TimeSpan(0, 0, 0);
        }

        public override double GetTimeLine()
        {
            if (_audioFile != null)
            {
                TimeSpan currentTime = _audioFile.CurrentTime;
                TimeSpan totalTime = _audioFile.TotalTime;

                int currentSeconds = currentTime.Hours * 60 * 60 + currentTime.Minutes * 60 + currentTime.Seconds;
                int totalSeconds = totalTime.Hours * 60 * 60 + totalTime.Minutes * 60 + totalTime.Seconds;

                double TimeLine = ((double)currentSeconds / (double)totalSeconds) * 100;

                return TimeLine;
            }
            else
                return 0;
        }

        public override void Mixed()
        {
            _isMixed = !_isMixed;

            if (_isMixed)
            {
                Playlist.CreateMixedSongList(_currentSong);
                _currentSong = 0;
            }
            else
            {
                if (Playlist.MixedTrackList.Count != 0)
                {
                    _currentSong = Playlist.TrackList.IndexOf(Playlist.MixedTrackList[_currentSong]);
                }
            }
        }

        public override void Pause()
        {
            _outputDevice.Pause();

            notifyPlayIcon(true);
        }

        public override void Play(int index)
        {
            if (_isMixed)
            {
                if (Playlist.MixedTrackList.Count != 0)
                {
                    if (PlayBackState() == 1) // ручное переключение трека (во время проигрывания другого)
                    {
                        _currentSong = Playlist.MixedTrackList.IndexOf(Playlist.TrackList[index]);

                        Play(Playlist.TrackList[index]);
                        notifyPlaylistView(index);
                    }
                    else // автоматическое переключение трека (при окончании предыдущего)
                    {
                        Play(Playlist.MixedTrackList[index]);
                        notifyPlaylistView(index);
                    }
                }
            }
            else if (Playlist.TrackList.Count != 0)
                Play(Playlist.TrackList[index]);
        }

        public override void Play(string name)
        {
            if (_isMixed)
            {
                if (PlayBackState() == 1) // ручное переключение трека (во время проигрывания другого)
                {
                    _currentSong = Playlist.MixedTrackList.IndexOf(name);

                    int index = Playlist.TrackList.IndexOf(name);
                    notifyPlaylistView(index);
                }
                else // автоматическое переключение трека (при окончании предыдущего)
                {
                    name = Playlist.MixedTrackList[_currentSong];

                    int index = Playlist.TrackList.IndexOf(name);
                    notifyPlaylistView(index);
                }
            }

            string fileName =  Playlist.GetFullFileName(name);

            if (_outputDevice != null)
            {
                _outputDevice.Stop();
            }

            if (!_isMixed)
            {
                _currentSong = Playlist.TrackList.IndexOf(name);
                notifyPlaylistView(_currentSong);
            }

            if (_outputDevice == null)
            {
                _outputDevice = new WaveOutEvent();
                _outputDevice.PlaybackStopped += OnSongStopped;
            }

            try
            {
                _audioFile = new AudioFileReader(@fileName);

                _outputDevice.Init(_audioFile);

                _outputDevice.Play();

                notifyPlayIcon(false);
                notifyTrackState(new ViewTrack(name, Playlist.TrackLibrary[name].image));
            }
            catch
            {
                DisposePlayback();

                ErrorManager.PlaybackError();
            }
        }

        private void OnSongStopped(object sender, StoppedEventArgs e)
        {
            if (PlayBackState() == -1)
                Next();
        }

        public override sbyte PlayBackState()
        {
            if (_outputDevice != null)
                return _outputDevice.PlaybackState == PlaybackState.Playing ? (sbyte)1 : _outputDevice.PlaybackState == PlaybackState.Paused ? (sbyte)0 : (sbyte)-1;
            else
                return -1;
            //                                      НЕ ТРОГАТЬ 
            //          Я ХЗ КАК ЭТО РАБОТАЕТ, Я ПИСАЛ ЭТО НА ПАРЕ БАШЛЫКОВОЙ, НЕ СПАВ НОЧЬ ПЕРЕД ЭТИМ)

            //          25.12.2019 Пол первого ночи... не знаю, кто это написал но он хорош

            //          Пара заметок: PlayBackState (1 play, 0 pause, -1 stop)
        }

        public override void Next()
        {
            if (_isRepeated)
            {
                Play(_currentSong);
            }
            else if (_currentSong < Playlist.TrackList.Count)
            {
                _currentSong++;

                if (_currentSong == Playlist.TrackList.Count)
                    _currentSong = 0;

                Play(_currentSong);
            }
        }

        public override void Previous()
        {
            _currentSong--;

            if (_currentSong == -1)
                _currentSong = 0;

            Play(_currentSong);
        }

        public override void Repeat()
        {
            _isRepeated = !_isRepeated;
        }

        public override void Resume()
        {
            _outputDevice.Play();
        }

        public override void ToTop()
        {
            if (_currentSong > 0)
            {
                string track = Playlist.TrackList[_currentSong];

                Playlist.TrackList.Remove(track);

                Playlist.TrackList.Insert(0, track);

                notifyToTopClick(_currentSong);

                _currentSong = 0;
            }
        }

        public override string GetCurrentTrack()
        {
            if (_currentSong != -1)
                return Playlist.TrackList[_currentSong];
            return null;
        }
        
        public void DisposePlayback()
        {
            if (_outputDevice != null)
            {
                _currentSong = -1;
                _outputDevice.Stop();
                _outputDevice.Dispose();
            }
            _outputDevice = null;
            if (_audioFile != null)
                _audioFile.Dispose();
            _audioFile = null;
        }


    }
}
