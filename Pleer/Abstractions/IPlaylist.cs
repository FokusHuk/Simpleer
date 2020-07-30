using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pleer.Models;

namespace Pleer.Abstractions
{
    public interface IPlaylist
    {
        List<string> TrackList { get; set; }

        Dictionary<string, Track> TrackLibrary { get; set; }

        List<string> MixedTrackList { get; set; }

        void CreateMixedSongList(int _currentSong);

        string GetFullFileName(string name);
        ViewTrack AddTrack(string file);
        List<ViewTrack> AddTrackList(string[] files);
        List<ViewTrack> AddTrackList(List<Track> tracks);
    }
}
