using AlvieEqualsysTestBackend.Models;

namespace AlvieEqualsysTestBackend.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(int id, Employee updated);
        Task<bool> DeleteAsync(int id);
    }
}
