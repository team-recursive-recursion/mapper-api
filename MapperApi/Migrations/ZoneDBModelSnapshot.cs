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
    [DbContext(typeof(ZoneDB))]
    partial class ZoneDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Mapper_Api.Models.Element", b =>
                {
                    b.Property<Guid?>("ElementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClassType")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("ElementType")
                        .IsRequired();

                    b.Property<string>("Info");

                    b.Property<byte[]>("Raw")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<Guid?>("ZoneID")
                        .IsRequired();

                    b.HasKey("ElementId");

                    b.HasIndex("ZoneID");

                    b.ToTable("Elements");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Element");
                });

            modelBuilder.Entity("Mapper_Api.Models.LiveLocation", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserID");

                    b.Property<byte[]>("PointRaw");

                    b.HasKey("CreatedAt", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("LiveLocation");
                });

            modelBuilder.Entity("Mapper_Api.Models.LiveUser", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("UserID");

                    b.ToTable("LiveUser");
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

                    b.Property<string>("Token");

                    b.HasKey("UserID");

                    b.HasAlternateKey("Email")
                        .HasName("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Mapper_Api.Models.Zone", b =>
                {
                    b.Property<Guid>("ZoneID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Info");

                    b.Property<Guid?>("ParentZoneID");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId");

                    b.Property<string>("ZoneName")
                        .IsRequired();

                    b.HasKey("ZoneID");

                    b.HasIndex("ParentZoneID");

                    b.HasIndex("UserId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("Mapper_Api.Models.Point", b =>
                {
                    b.HasBaseType("Mapper_Api.Models.Element");


                    b.ToTable("Point");

                    b.HasDiscriminator().HasValue("Point");
                });

            modelBuilder.Entity("Mapper_Api.Models.Polygon", b =>
                {
                    b.HasBaseType("Mapper_Api.Models.Element");


                    b.ToTable("Polygon");

                    b.HasDiscriminator().HasValue("Polygon");
                });

            modelBuilder.Entity("Mapper_Api.Models.Element", b =>
                {
                    b.HasOne("Mapper_Api.Models.Zone", "Zone")
                        .WithMany("Elements")
                        .HasForeignKey("ZoneID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mapper_Api.Models.LiveLocation", b =>
                {
                    b.HasOne("Mapper_Api.Models.LiveUser")
                        .WithMany("Locations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mapper_Api.Models.Zone", b =>
                {
                    b.HasOne("Mapper_Api.Models.Zone", "ParentZone")
                        .WithMany("InnerZones")
                        .HasForeignKey("ParentZoneID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mapper_Api.Models.User")
                        .WithMany("Zones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
