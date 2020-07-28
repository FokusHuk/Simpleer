using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Pleer.Abstractions;
using Pleer.Models;

namespace Pleer.ViewModels
{
    public class PleerPanelViewModel: Caliburn.Micro.Screen
    {
        PlaybackBase playback;

        private PackIconKind _playIcon;
        private PackIconKind _muteIcon;
        private string _fullSongTime;
        private string _currentSongTime;

        // Volume
        private float _volume = 3.0f;
        private float _savedVolume = 0.0f;

        // TimeLine
        private double _timeLine = 0.0;
        private System.Windows.Threading.DispatcherTimer _timer;
        private bool _autoChanged = false;

        public PleerPanelViewModel()
        {
            playback = Playback.Instance();

            playback.notifyPlayIcon += PlayIconNotify;

            PlayIcon = PackIconKind.Play;
            MuteIcon = PackIconKind.VolumeHigh;
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            _timer.Start();
        }

        public PackIconKind PlayIcon
        {
            get { return _playIcon; }
            set
            {
                _playIcon = value;

                NotifyOfPropertyChange(() => PlayIcon);
            }
        }

        public PackIconKind MuteIcon
        {
            get { return _muteIcon; }
            set
            {
                _muteIcon = value;

                NotifyOfPropertyChange(() => MuteIcon);
            }
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                if (value > 10.0f)
                    _volume = 10.0f;
                else if (value < 0.0f)
                    _volume = 0.0f;
                else
                    _volume = value;

                playback.ChangeVolume(_volume / 10.0f);

                NotifyOfPropertyChange(() => Volume);
            }
        }

        public double TimeLine
        {
            get { return _timeLine; }
            set
            {
                if (value > 100.0)
                    _timeLine = 100.0;
                else if (value < 0.0)
                    _timeLine = 0.0;
                else
                    _timeLine = value;

                if (!_autoChanged)
                {
                    playback.ChangeTimeLine(_timeLine / 100);
                }

                NotifyOfPropertyChange(() => TimeLine);

                if (_autoChanged)
                    _autoChanged = false;

                TimeSpan totalTime = playback.GetFullSongTime();
                if (totalTime.Minutes != 0 || totalTime.Seconds != 0)
                {
                    if (totalTime.Seconds < 10)
                        FullSongTime = totalTime.Minutes + ":0" + totalTime.Seconds;
                    else
                        FullSongTime = totalTime.Minutes + ":" + totalTime.Seconds;
                }
                else
                {
                    FullSongTime = "00:00";
                }
                TimeSpan curentTime = playback.GetCurrentSongTime();
                if (curentTime.Minutes != 0 || curentTime.Seconds != 0)
                {
                    if (curentTime.Seconds < 10)
                        CurrentSongTime = "0" + curentTime.Minutes + ":0" + curentTime.Seconds;
                    else
                        CurrentSongTime = "0" + curentTime.Minutes + ":" + curentTime.Seconds;
                }
                else
                {
                    CurrentSongTime = "00:00";
                }
            }
        }

        public string FullSongTime
        {
            get { return _fullSongTime; }
            set
            {
                _fullSongTime = value;

                NotifyOfPropertyChange(() => FullSongTime);
            }
        }

        public string CurrentSongTime
        {
            get { return _currentSongTime; }
            set
            {
                _currentSongTime = value;

                NotifyOfPropertyChange(() => CurrentSongTime);
            }
        }



        public void PlaySong()
        {
            if (playback.PlayBackState() == 1)
            {
                playback.Pause();
            }
            else if (playback.PlayBackState() == 0)
            {
                playback.Resume();
            }
        }

        public void PlayNext()
        {
            playback.Next();
        }

        public void PlayPrevious()
        {
            playback.Previous();
        }

        public void Mute()
        {
            if (Volume == 0 && _savedVolume != 0.0f)
            {
                Volume = _savedVolume;
                _savedVolume = 0.0f;
                MuteIcon = PackIconKind.VolumeHigh;
            }
            else if (Volume != 0.0f)
            {
                _savedVolume = Volume;
                Volume = 0.0f;
                MuteIcon = PackIconKind.VolumeOff;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _autoChanged = true;
            TimeLine = playback.GetTimeLine();
        }

        public void Repeat()
        {
            playback.Repeat();
        }

        public void Mixed()
        {
            playback.Mixed();
        }

        public void FindLyrics()
        {
            string name = playback.GetCurrentTrack();
            if (name != null)
                System.Diagnostics.Process.Start("https://www.google.ru/?gws_rd=ssl#newwindow=1&q=" + name + " lyrics");
        }

        public void ToTop()
        {
            playback.ToTop();
        }


        public void PlayIconNotify(bool param)
        {
            if (param)
            {
                PlayIcon = PackIconKind.Play;
            }
            else
            {
                PlayIcon = PackIconKind.Pause;
            }
        }
    }
}
