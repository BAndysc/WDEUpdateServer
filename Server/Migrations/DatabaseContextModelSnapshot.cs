﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Services.Database;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Server.Models.Database.ChangeLogEntryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Change")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("VersionId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("VersionId");

                    b.ToTable("ChangelogEntries");
                });

            modelBuilder.Entity("Server.Models.Database.CommentModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Approved")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Key");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Server.Models.Database.FileEntityModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("HashMd5")
                        .HasColumnType("longtext");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UploaderUser")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Key");

                    b.HasIndex("UploaderUser");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Server.Models.Database.MarketplaceModel", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.HasKey("Name");

                    b.ToTable("Marketplaces");
                });

            modelBuilder.Entity("Server.Models.Database.StaticFileModel", b =>
                {
                    b.Property<uint>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Key");

                    b.ToTable("StaticFiles");
                });

            modelBuilder.Entity("Server.Models.Database.UserModel", b =>
                {
                    b.Property<string>("User")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("KeyHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("User");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Server.Models.Database.VersionEntityModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Marketplace")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TextVersion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UpdateTitle")
                        .HasColumnType("longtext");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Branch");

                    b.HasIndex("Marketplace");

                    b.HasIndex("Version");

                    b.ToTable("Versions");
                });

            modelBuilder.Entity("Server.Models.Database.VersionFilesModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FileKey")
                        .HasColumnType("char(36)");

                    b.Property<int>("Platform")
                        .HasColumnType("int");

                    b.Property<Guid>("VersionId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FileKey");

                    b.HasIndex("VersionId");

                    b.ToTable("VersionFiles");
                });

            modelBuilder.Entity("Server.Models.Database.ChangeLogEntryModel", b =>
                {
                    b.HasOne("Server.Models.Database.VersionEntityModel", "Version")
                        .WithMany("Changes")
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Version");
                });

            modelBuilder.Entity("Server.Models.Database.FileEntityModel", b =>
                {
                    b.HasOne("Server.Models.Database.UserModel", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("Server.Models.Database.VersionFilesModel", b =>
                {
                    b.HasOne("Server.Models.Database.FileEntityModel", "File")
                        .WithMany("ReferencedVersions")
                        .HasForeignKey("FileKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Database.VersionEntityModel", "Version")
                        .WithMany("Files")
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Version");
                });

            modelBuilder.Entity("Server.Models.Database.FileEntityModel", b =>
                {
                    b.Navigation("ReferencedVersions");
                });

            modelBuilder.Entity("Server.Models.Database.VersionEntityModel", b =>
                {
                    b.Navigation("Changes");

                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
