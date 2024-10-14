using Microsoft.EntityFrameworkCore;
using SkillService.Models;

namespace SkillService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Certificate>()
                .HasMany(p => p.Skills)
                .WithOne(p => p.Certificate!)
                .HasForeignKey(p => p.CertificateId);

            modelBuilder
                .Entity<Skill>()
                .HasOne(p => p.Certificate)
                .WithMany(p => p.Skills)
                .HasForeignKey(p => p.CertificateId);
        }
    }
}
