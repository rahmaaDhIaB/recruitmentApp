using App.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace App.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Offre>? Offres { get; set; }

        public virtual  DbSet<candidature>? Candidatures { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-many relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Offres)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);
        }



    }




}
