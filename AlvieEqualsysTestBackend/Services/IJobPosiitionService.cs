using AlvieEqualsysTestBackend.Models;

namespace AlvieEqualsysTestBackend.Services
{
    public interface IJobPositionService
    {
        Task<List<JobPosition>> GetByEmployeeIdAsync(int employeeId);
        Task<JobPosition> AddToEmployeeAsync(int employeeId, JobPosition job);
        Task<JobPosition?> UpdateAsync(int id, JobPosition updated);
        Task<bool> DeleteAsync(int id);
    }
}
