using Clean_Pakistan.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Clean_Pakistan.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database Tables
        public DbSet<User> Users { get; set; }
        public DbSet<CivicIssue> CivicIssues { get; set; }
        public DbSet<IssueVerification> IssueVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Constraint: Prevents a single citizen from voting/verifying the exact same issue twice
            modelBuilder.Entity<IssueVerification>()
                .HasIndex(v => new { v.IssueId, v.UserId })
                .IsUnique();

         modelBuilder.Entity<IssueVerification>()
        .HasOne(v => v.User)
        .WithMany()
        .HasForeignKey(v => v.UserId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}