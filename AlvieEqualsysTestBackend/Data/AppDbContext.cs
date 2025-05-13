using AlvieEqualsysTestBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AlvieEqualsysTestBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<JobPosition> JobPositions => Set<JobPosition>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=employee.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobPosition>()
                .HasOne(j => j.Employee)
                .WithMany(e => e.JobPositions)
                .HasForeignKey(j => j.EmployeeId);
        }
    }
}
