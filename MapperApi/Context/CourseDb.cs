/***
 * Filename: CourseDb.cs
 * Author : ebendutoit
 * Class   : CourseDb
 *
 *      Database context for API 
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

        public DbSet<GolfCourse> GolfCourses { get; set; }
        public DbSet<CoursePolygon> CoursePolygons { get; set; }

        public DbSet<Point> Point { get; set; }

        public DbSet<CourseElement> CourseElement { get; set; }

        public DbSet<Hole> Hole { get; set; }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GolfCourse>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<GolfCourse>()
                    .Property(b => b.UpdatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<CoursePolygon>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<CoursePolygon>()
                    .Property(b => b.UpdatedAt)
                    .HasDefaultValueSql("now()");

            modelBuilder.Entity<User>()
                    .HasAlternateKey(c => c.Email)
                    .HasName("Email");
        }
    }
}