using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pleer.Models
{
    public class ViewTrack
    {
        public string Name { get; set; }
        public BitmapImage TrackImage { get; set; }

        public ViewTrack(string name, BitmapImage trackImage)
        {
            Name = name;
            TrackImage = trackImage;
        }
    }
}
