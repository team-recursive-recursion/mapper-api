/***
 * Filename: CourseDb.cs
 * Author  : Eben Du Toit, Duncan Tilley
 * Class   : CourseDb
 *
 *      Database context for API.
 ***/
using Mapper_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Context
{
    public class CourseDb : DbContext
    {
        public CourseDb(DbContextOptions<CourseDb> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Polygon> Polygons { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<Element> Elements { get; set; }

        public DbSet<Hole> Holes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<LiveLocation> LiveLocation { get; set; }

        public DbSet<LiveUser> LiveUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
            .Property(b => b.CreatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Course>()
                    .Property(b => b.UpdatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<Polygon>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<Polygon>()
                    .Property(b => b.UpdatedAt)
                    .HasDefaultValueSql("now()");


            modelBuilder.Entity<LiveLocation>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<LiveLocation>().HasKey(ll => new {
                ll.CreatedAt, 
                ll.UserID});

            modelBuilder.Entity<User>()
                    .HasAlternateKey(c => c.Email)
                    .HasName("Email");
        }
    }
}
