﻿// <auto-generated />
using System;
using MemeFolder.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MemeFolder.EntityFramework.Migrations
{
    [DbContext(typeof(MemeFolderDbContext))]
    [Migration("20210323143723_odu4")]
    partial class odu4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("MemeFolder.Domain.Models.Folder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageFolderPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentFolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.Meme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("Memes");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.MemeTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MemeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.HasIndex("MemeId");

                    b.ToTable("MemeTags");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.Folder", b =>
                {
                    b.HasOne("MemeFolder.Domain.Models.Folder", "ParentFolder")
                        .WithMany("Folders")
                        .HasForeignKey("ParentFolderId");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.Meme", b =>
                {
                    b.HasOne("MemeFolder.Domain.Models.Folder", "Folder")
                        .WithMany("Memes")
                        .HasForeignKey("FolderId");

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.MemeTag", b =>
                {
                    b.HasOne("MemeFolder.Domain.Models.Folder", null)
                        .WithMany("Tags")
                        .HasForeignKey("FolderId");

                    b.HasOne("MemeFolder.Domain.Models.Meme", null)
                        .WithMany("Tags")
                        .HasForeignKey("MemeId");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.Folder", b =>
                {
                    b.Navigation("Folders");

                    b.Navigation("Memes");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("MemeFolder.Domain.Models.Meme", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
