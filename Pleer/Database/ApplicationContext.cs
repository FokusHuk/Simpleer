using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Pleer.Models;

namespace Pleer.Database
{
    public partial class ApplicationContext : DbContext
    {
        public DbSet<Track> AllTracks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AllTracksConfiguration());
        }
    }

    public class AllTracksConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.path).IsRequired();
            builder.Property(t => t.name).IsRequired();
            builder.Property(t => t.format).IsRequired();
            builder.Ignore(t => t.image);
        }
    }
}