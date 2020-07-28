using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pleer.Models
{
    public class Track
    {
        public int Id { get; private set; }
        public string path { get; set; }
        public string format { get; set; }
        public string name { get; set; }
        public BitmapImage image { get; set; }

        public Track(string name, string path, string format, BitmapImage image)
        {
            this.name = name;
            this.path = path;
            this.format = format;
            this.image = image;
        }

        public Track(string name, string path, string format)
        {
            this.name = name;
            this.path = path;
            this.format = format;
            this.image = null;
        }
    }
}
