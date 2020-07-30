using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Pleer.Models;
using Pleer.Abstractions;
using System.Collections.Generic;

namespace Pleer.Database
{
    public class DbManager
    {
        #region Singleton
        private static DbManager dbmanager;

        static DbManager()
        {
            dbmanager = new DbManager();
        }

        private DbManager()
        {

        }

        public static DbManager Instance()
        {
            return dbmanager;
        }
        #endregion

        DbContextOptions<ApplicationContext> Options;

        public void Init()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            Options = optionsBuilder.UseSqlServer(connectionString).Options;
        }

        public void LoadData(IPlaylistViewModel musicPanel)
        {
            List<Track> tracks = null;

            using (ApplicationContext db = new ApplicationContext(Options))
            {
                tracks = db.AllTracks.ToList();
            }

            musicPanel.AddTracksFromDb(tracks);
        }

        public void SaveTracks(List<Track> newTracks)
        {
            using (ApplicationContext db = new ApplicationContext(Options))
            {
                for (int i = newTracks.Count - 1; i >= 0; i--)
                {
                    db.AllTracks.Add(newTracks[i]);
                }
                db.SaveChanges();
            }
        }

        public void SaveTrack(Track newTrack)
        {
            using (ApplicationContext db = new ApplicationContext(Options))
            {
                db.AllTracks.Add(newTrack);
                db.SaveChanges();
            }
        }
    }
}
