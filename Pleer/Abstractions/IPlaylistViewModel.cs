using Pleer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleer.Abstractions
{
    public interface IPlaylistViewModel
    {
        void AddTrackFromFolder();
        void AddTracksFromDb(List<Track> tracks);
    }
}
