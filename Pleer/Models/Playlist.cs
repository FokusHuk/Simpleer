using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Pleer.Abstractions;
using Pleer.Properties;

namespace Pleer.Models
{
    class Playlist : IPlaylist
    {
        private BitmapImage baseTrackImage;

        // Упорядоченный список треков
        public List<string> TrackList { get; set; }

        // Словарь всех треков с информацией
        public Dictionary<string, Track> TrackLibrary { get; set; }

        // Перемешанный список треков
        public List<string> MixedTrackList { get; set; }


        public Playlist()
        {
            TrackLibrary = new Dictionary<string, Track>();
            TrackList = new List<string>();
        }

        // Проверка наличия трека в библиотеке
        private bool IsTrackExist(string name)
        {
            return TrackLibrary.ContainsKey(name);
        }

        // Полное имя трека (путь + имя + расширение)
        public string GetFullFileName(string name)
        {
            Track track;

            if (!TrackLibrary.TryGetValue(name, out track))
            {
                throw new FileNotFoundException();
            }

            string fullName = track.path + name + track.format;

            return fullName;
        }



        // Добавление треков из директории
        public List<ViewTrack> LoadFromDirectory(string directoryPath)
        {
            string[] tracks = Directory.GetFiles(@directoryPath, "*.mp3", SearchOption.AllDirectories);

            return AddTrackList(tracks);
        }

        public void CreateMixedSongList(int _currentSong)
        {
            int startSongIndex = 0;

            if (_currentSong != -1)
                startSongIndex = _currentSong;

            Random random = new Random();

            MixedTrackList = new List<string>();
            for (int i = 0; i < TrackList.Count; i++)
                MixedTrackList.Add(TrackList[i]);

            for (int i = TrackList.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);

                var temp = MixedTrackList[j];
                MixedTrackList[j] = MixedTrackList[i];
                MixedTrackList[i] = temp;
            }

            for (int i = 0; i < TrackList.Count; i++)
                if (MixedTrackList[i] == TrackList[startSongIndex])
                {
                    var temp = MixedTrackList[i];
                    MixedTrackList[i] = MixedTrackList[0];
                    MixedTrackList[0] = temp;
                }
        }

        // Добавление списка треков [переработано]
        public List<ViewTrack> AddTrackList(string[] files)
        {
            files = files.Reverse().ToArray<string>();

            List<ViewTrack> viewTracks = new List<ViewTrack>();

            string fullTrackName = "";
            string trackName = "";
            string trackFormat = "";

            foreach (string file in files)
            {
                fullTrackName = Path.GetFileName(file);
                trackName = fullTrackName.Substring(0, fullTrackName.LastIndexOf('.'));

                if (!IsTrackExist(trackName))
                {
                    trackFormat = fullTrackName.Substring(fullTrackName.LastIndexOf('.'));

                    BitmapImage image = getTrackImage(file);

                    Track newTrack = new Track(trackName, Path.GetDirectoryName(file) + "\\", trackFormat, image);

                    TrackLibrary.Add(trackName, newTrack);
                    TrackList.Insert(0, trackName);

                    viewTracks.Add(new ViewTrack(trackName, image));
                }
            }

            return viewTracks;
        }

        // Добавление одного трека [переработано]
        public ViewTrack AddTrack(string filePath)
        {
            string fullTrackName = Path.GetFileName(filePath);
            string trackName = fullTrackName.Substring(0, fullTrackName.LastIndexOf('.'));

            if (!IsTrackExist(trackName))
            {
                string trackFormat = fullTrackName.Substring(fullTrackName.LastIndexOf('.'));

                BitmapImage image = getTrackImage(filePath);

                Track newTrack = new Track(trackName, Path.GetDirectoryName(filePath) + "\\", trackFormat, image);

                TrackLibrary.Add(trackName, newTrack);
                TrackList.Insert(0, trackName);

                return new ViewTrack(trackName, image);
            }

            return null;
        }

        // Получение изображения трека
        private BitmapImage getTrackImage(string filePath)
        {
            var tagSong = TagLib.File.Create(filePath);

            byte[] byteArrayIn;
            BitmapImage image = new BitmapImage();

            if (tagSong.Tag.Pictures.Length != 0)
            {
                byteArrayIn = tagSong.Tag.Pictures[0].Data.Data;
            }
            else if (baseTrackImage == null)
            {
                baseTrackImage = image;

                var img = Properties.Resources.NoImageIcon;

                using (var stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    byteArrayIn = stream.ToArray();
                }
            }
            else
            {
                return baseTrackImage;
            }

            MemoryStream strmImg = new MemoryStream(byteArrayIn);

            try
            {
                image.BeginInit();
                image.StreamSource = strmImg;
                image.DecodePixelWidth = 200;
                image.EndInit();
            }
            catch
            {
                // TODO new Exception
                return null;
            }

            return image;
        }
    }

}
