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
        //sina



















































        //alireza
        public DbSet<User> Users { set; get; }



















































        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //sina

























            //alireza


            modelBuilder.Entity<User>()
                 .HasIndex(u => new { u.Phone }).IsUnique();

/*            modelBuilder.Entity<User>()
                .HasMany(u => u.refreshTokens)
                .WithOne(r => r.user)
                .HasForeignKey(r => r.user_id)
                .IsRequired();*/



































































            ApplyQueryFilter(modelBuilder);
        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            //sina


















            //alireza
















        }




    }


}