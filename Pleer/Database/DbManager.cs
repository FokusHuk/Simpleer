using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Pleer.Models;

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
    }
}
