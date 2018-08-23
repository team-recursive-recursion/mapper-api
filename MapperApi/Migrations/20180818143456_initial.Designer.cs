﻿// <auto-generated />
using Mapper_Api.Context;
using Mapper_Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace MapperApi.Migrations
{
    [DbContext(typeof(CourseDb))]
    [Migration("20180818143456_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Mapper_Api.Models.Course", b =>
                {
                    b.Property<Guid>("CourseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CourseName")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId");

                    b.HasKey("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Mapper_Api.Models.Element", b =>
                {
                    b.Property<Guid>("ElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CourseId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("ElementType");

                    b.Property<Guid?>("HoleId");

                    b.HasKey("ElementId");

                    b.HasIndex("CourseId");

                    b.HasIndex("HoleId");

                    b.ToTable("Elements");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Element");
                });

            modelBuilder.Entity("Mapper_Api.Models.Hole", b =>
                {
                    b.Property<Guid>("HoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CourseId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("HoleId");

                    b.HasIndex("CourseId");

                    b.ToTable("Holes");
                });

            modelBuilder.Entity("Mapper_Api.Models.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("UserID");

                    b.HasAlternateKey("Email")
                        .HasName("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Mapper_Api.Models.Point", b =>
                {
                    b.HasBaseType("Mapper_Api.Models.Element");

                    b.Property<string>("Info");

                    b.Property<byte[]>("PointRaw")
                        .IsRequired();

                    b.Property<int>("PointType");

                    b.ToTable("Point");

                    b.HasDiscriminator().HasValue("Point");
                });

            modelBuilder.Entity("Mapper_Api.Models.Polygon", b =>
                {
                    b.HasBaseType("Mapper_Api.Models.Element");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<byte[]>("PolygonRaw")
                        .IsRequired();

                    b.Property<int>("PolygonType");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.ToTable("Polygon");

                    b.HasDiscriminator().HasValue("Polygon");
                });

            modelBuilder.Entity("Mapper_Api.Models.Course", b =>
                {
                    b.HasOne("Mapper_Api.Models.User")
                        .WithMany("Courses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mapper_Api.Models.Element", b =>
                {
                    b.HasOne("Mapper_Api.Models.Course")
                        .WithMany("Elements")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mapper_Api.Models.Hole")
                        .WithMany("Elements")
                        .HasForeignKey("HoleId");
                });

            modelBuilder.Entity("Mapper_Api.Models.Hole", b =>
                {
                    b.HasOne("Mapper_Api.Models.Course")
                        .WithMany("Holes")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
