using AlvieEqualsysTestBackend.Data;
using AlvieEqualsysTestBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AlvieEqualsysTestBackend.Services
{
    public class JobPositionService : IJobPositionService
    {
        private readonly AppDbContext _db;

        public JobPositionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<JobPosition>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _db.JobPositions
                .Where(j => j.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<JobPosition> AddToEmployeeAsync(int employeeId, JobPosition job)
        {
            // Ensure only one active job per employee
            if (job.Status == "Active")
            {
                var hasActive = await _db.JobPositions
                    .AnyAsync(j => j.EmployeeId == employeeId && j.Status == "Active");

                if (hasActive)
                    throw new InvalidOperationException("Employee already has an active job.");
            }

            job.EmployeeId = employeeId;
            _db.JobPositions.Add(job);
            await _db.SaveChangesAsync();
            return job;
        }

        public async Task<JobPosition?> UpdateAsync(int id, JobPosition updated)
        {
            var existing = await _db.JobPositions.FindAsync(id);
            if (existing == null) return null;

            if (updated.Status == "Active" && existing.Status != "Active")
            {
                var hasActive = await _db.JobPositions
                    .AnyAsync(j => j.EmployeeId == existing.EmployeeId && j.Status == "Active" && j.Id != id);

                if (hasActive)
                    throw new InvalidOperationException("Employee already has another active job.");
            }

            existing.JobName = updated.JobName;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.Salary = updated.Salary;
            existing.Status = updated.Status;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var job = await _db.JobPositions.FindAsync(id);
            if (job == null) return false;

            _db.JobPositions.Remove(job);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
