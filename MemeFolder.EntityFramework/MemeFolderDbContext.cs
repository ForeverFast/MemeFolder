using MemeFolder.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace MemeFolder.EntityFramework
{
    public class MemeFolderDbContext : DbContext
    {

        public DbSet<Meme> Memes { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<MemeTag> MemeTags { get; set; }
        public DbSet<MemeTagNode> MemeTagNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>(entity => {

                entity.HasMany(f => f.Folders);
                entity.HasOne(f => f.ParentFolder);
                        //.WithMany(pf => pf.Folders)
                        //.HasForeignKey(f => f.ParentId)
                        //.IsRequired(false)
                        //.OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(f => f.Memes);
            });


            modelBuilder.Entity<Meme>(entity =>
            {
                entity.HasMany(m => m.Tags);
                entity.HasOne(m => m.Folder);
            });

            modelBuilder.Entity<MemeTagNode>(entity =>
            {
                entity.HasOne(m => m.MemeTag);
                entity.HasOne(m => m.Meme);
            });

            
        }

        public MemeFolderDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
