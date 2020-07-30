using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Caliburn.Micro;
using Pleer.Abstractions;
using Pleer.Models;

namespace Pleer.ViewModels
{
    public class AllMusicViewModel : Caliburn.Micro.Screen, IPlaylistViewModel
    {
        private PlaybackBase playback;
        private IPlaylist playlist;
        private BindableCollection<ViewTrack> _trackList = new BindableCollection<ViewTrack>();
        private int _selectedIndex;


        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyOfPropertyChange(() => SelectedIndex);
            }

        }

        public BindableCollection<ViewTrack> TrackList
        {
            get { return _trackList; }
            set { _trackList = value; }
        }


        public AllMusicViewModel()
        {
            playback = Playback.Instance();
            playlist = new Playlist();
        }

        
        public void AddTrackFromFolder()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3|All files (*.*)|*.*";

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;

                if (files.Length == 1)
                {
                    ViewTrack newTrack = playlist.AddTrack(files[0]);

                    if (newTrack != null)
                    {
                        TrackList.Insert(0, newTrack);
                    }
                }
                else if (files.Length > 1)
                {
                    List<ViewTrack> newTracks = playlist.AddTrackList(files);

                    foreach (var track in newTracks)
                        TrackList.Insert(0, track);
                }
            }
        }

        public void AddTracksFromDb(List<Track> tracks)
        {
            List<ViewTrack> newTracks = playlist.AddTrackList(tracks);

            foreach (var track in newTracks)
                TrackList.Insert(0, track);
        }


        public void PlayMethod()
        {
            if (TrackList != null)
                if (TrackList.Count != 0 && SelectedIndex <= TrackList.Count && SelectedIndex >= 0)
                {
                    if (playback.Playlist != playlist)
                    {
                        playback.Playlist = playlist;
                        playback.notifyPlaylistView += NotifySelected;
                        playback.notifyToTopClick += NotifyToTop;
                    }
                    playback.Play(SelectedIndex);
                }
        }


        private void NotifySelected(int index)
        {
            SelectedIndex = index;
        }


        private void NotifyToTop(int index)
        {
            ViewTrack track = TrackList[index];

            TrackList.Remove(track);

            TrackList.Insert(0, track);

            SelectedIndex = 0;
        }

    }
}
