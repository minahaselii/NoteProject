using System.Data;
using Microsoft.EntityFrameworkCore;
using NoteProject.Entity;

namespace NoteProject.Context
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }




        public DbSet<User> Users { set; get; }
        public DbSet<Note> Notes { set; get; }
        public DbSet<Like> Likes { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<User>()
                 .HasIndex(u => new { u.Phone }).IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.notes)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .IsRequired();

            ApplyQueryFilter(modelBuilder);
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {


        }

    }
}