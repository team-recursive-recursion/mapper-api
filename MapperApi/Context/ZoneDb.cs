/***
 * Filename: ZoneDB.cs
 * Author  : Eben Du Toit, Duncan Tilley
 * Class   : ZoneDB
 *
 *      Database context for API.
 ***/
using Mapper_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Context
{
    public class ZoneDB : DbContext
    {
        public ZoneDB(DbContextOptions<ZoneDB> options) : base(options) { }

        // Content
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<Polygon> Polygons { get; set; }
        public DbSet<Point> Points { get; set; }

        // Content Users
        public DbSet<LiveLocation> LiveLocation { get; set; }
        public DbSet<LiveUser> LiveUser { get; set; }

        // Content creators
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Zone>()
            .Property(b => b.CreatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Zone>()
                    .Property(b => b.UpdatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<Element>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<Element>()
                    .Property(b => b.UpdatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<LiveLocation>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<LiveLocation>().HasKey(ll => new
            {
                ll.CreatedAt,
                ll.UserID
            });

            modelBuilder.Entity<User>()
                    .HasAlternateKey(c => c.Email)
                    .HasName("Email");
        }
    }
}
