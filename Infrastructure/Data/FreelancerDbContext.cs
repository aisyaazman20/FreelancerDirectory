using Microsoft.EntityFrameworkCore;
using Core.Entities;


namespace Infrastructure.Data
{
    public class FreelancerDbContext : DbContext
    {
        //contstructor for dependency injection
        public FreelancerDbContext(DbContextOptions<FreelancerDbContext> options)
            : base(options)
        {
        }

        //dbset: create table for each entity
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }

        //configure db relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Freelancer to Skill relationship
            modelBuilder.Entity<Freelancer>()
                .HasMany(f => f.Skills)
                .WithOne(s => s.Freelancer)
                .HasForeignKey(s => s.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Freelancer to Hobby relationship
            modelBuilder.Entity<Freelancer>()
                .HasMany(f => f.Hobbies)
                .WithOne(h => h.Freelancer)
                .HasForeignKey(h => h.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            // unique constraint on Username and Email
            modelBuilder.Entity<Freelancer>()
                .HasIndex(f => f.Username)
                .IsUnique();

            modelBuilder.Entity<Freelancer>()
                .HasIndex(f => f.Email)
                .IsUnique();
        }
    }
}
