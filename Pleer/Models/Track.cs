using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pleer.Models
{
    public struct Track
    {
        public string path;
        public string format;
        public string name;
        public BitmapImage image;

        public Track(string name, string path, string format, BitmapImage image)
        {
            this.name = name;
            this.path = path;
            this.format = format;
            this.image = image;
        }
    }
}
