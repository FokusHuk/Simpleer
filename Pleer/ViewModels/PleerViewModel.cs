using Caliburn.Micro;
using Pleer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TagLib.Id3v2;
using System.Windows.Media.Imaging;
using System.Timers;
using MaterialDesignThemes.Wpf;
using Pleer.Abstractions;
using Pleer.Database;

namespace Pleer.ViewModels
{

    public class PleerViewModel : Conductor<Screen>.Collection.AllActive
    {
        AllMusicViewModel allMusicViewModel;
        IPlaylistViewModel musicPanel;
        private BitmapImage _trackImage;
        private string _trackName;
        private Screen _bottomPanel;
        private Screen _centralPanel;

        public Screen BottomPanel
        {
            get { return _bottomPanel; }
            set { _bottomPanel = value; }
        }

        public Screen CentralPanel
        {
            get { return _centralPanel; }
            set
            {
                _centralPanel = value;
                NotifyOfPropertyChange(() => CentralPanel);
            }
        }

        public BitmapImage TrackImage
        {
            get { return _trackImage; }
            set
            {
                _trackImage = value;

                NotifyOfPropertyChange(() => TrackImage);
            }
        }

        public string TrackName
        {
            get { return _trackName; }
            set
            {
                _trackName = value;

                NotifyOfPropertyChange(() => TrackName);
            }
        }



        public PleerViewModel()
        {
            allMusicViewModel = new AllMusicViewModel();
            
            CentralPanel = allMusicViewModel;
            Items.Add(CentralPanel);

            musicPanel = allMusicViewModel;

            BottomPanel = new PleerPanelViewModel();
            Items.Add(BottomPanel);

            Playback.Instance().notifyTrackState += NotifyTrackState;

            DbManager db = DbManager.Instance();
            db.Init();
            db.LoadData(allMusicViewModel);
        }

        public void Settings()
        {
            CentralPanel = new SettingsViewModel();
            
        }

        public void Playlists()
        {
            CentralPanel = new PlaylistsViewModel();
        }

        public void Popular()
        {
            CentralPanel = new PopularViewModel();
        }

        public void Stats()
        {
            CentralPanel = new StatsViewModel();
        }

        public void AllMusic()
        {
            CentralPanel = allMusicViewModel;
            musicPanel = allMusicViewModel;
        }

        public void AddTrack()
        {
            if (musicPanel != null)
            {
                musicPanel.AddTrackFromFolder();
            }
        }


        public void NotifyTrackState(ViewTrack track)
        {
            TrackImage = track.TrackImage;
            TrackName = track.Name;
        }

    }
}
