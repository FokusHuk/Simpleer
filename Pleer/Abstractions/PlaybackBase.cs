using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pleer.Models;

namespace Pleer.Abstractions
{
    public abstract class PlaybackBase
    {
        public IPlaylist Playlist { get; set; }

        public PlaybackBase()
        {

        }

        public delegate void NotifyPlaylistView(int index);
        private NotifyPlaylistView _notifyPlaylistView;
        public NotifyPlaylistView notifyPlaylistView
        {
            get { return _notifyPlaylistView; }
            set
            {
                if (_notifyPlaylistView == null)
                    _notifyPlaylistView += value;
                else if (_notifyPlaylistView.Method != value.Method)
                {
                    _notifyPlaylistView = null;
                    _notifyPlaylistView += value;
                }
            }
        }

        public delegate void NotifyPlayIcon(bool param);
        private NotifyPlayIcon _notifyPlayIcon;
        public NotifyPlayIcon notifyPlayIcon
        {
            get { return _notifyPlayIcon; }
            set
            {
                if (_notifyPlayIcon == null)
                    _notifyPlayIcon += value;
                else if (_notifyPlayIcon.Method != value.Method)
                {
                    _notifyPlayIcon = null;
                    _notifyPlayIcon += value;
                }
            }
        }

        public delegate void NotifyTrackState(ViewTrack track);
        private NotifyTrackState _notifyTrackState;
        public NotifyTrackState notifyTrackState
        {
            get { return _notifyTrackState; }
            set
            {
                if (_notifyTrackState == null)
                    _notifyTrackState += value;
                else if (_notifyTrackState.Method != value.Method)
                {
                    _notifyTrackState = null;
                    _notifyTrackState += value;
                }
            }
        }

        public delegate void NotifyToTopClick(int index);
        private NotifyToTopClick _notifyToTopClick;
        public NotifyToTopClick notifyToTopClick
        {
            get { return _notifyToTopClick; }
            set
            {
                if (_notifyToTopClick == null)
                    _notifyToTopClick += value;
                else if (_notifyToTopClick.Method != value.Method)
                {
                    _notifyToTopClick = null;
                    _notifyToTopClick += value;
                }
            }
        }

        public abstract void Play(int index);

        public abstract void Play(string name);

        public abstract void Resume();

        public abstract void Pause();

        public abstract void Next();

        public abstract void Previous();

        public abstract void ChangeVolume(float volume);

        public abstract void ChangeTimeLine(double timeLine);

        public abstract double GetTimeLine();

        public abstract void Repeat();

        public abstract void Mixed();

        public abstract void ToTop();

        public abstract string GetCurrentTrack();

        public abstract sbyte PlayBackState();

        public abstract TimeSpan GetFullSongTime();

        public abstract TimeSpan GetCurrentSongTime();
    }
}
