using AlvieEqualsysTestBackend.Data;
using AlvieEqualsysTestBackend.Models;

namespace AlvieEqualsysTestBackend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _db;

        public EmployeeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _db.Employees
                .Include(e => e.JobPositions)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _db.Employees
                .Include(e => e.JobPositions)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> UpdateAsync(int id, Employee updated)
        {
            var existing = await _db.Employees.FindAsync(id);
            if (existing == null) return null;

            existing.FirstName = updated.FirstName;
            existing.MiddleName = updated.MiddleName;
            existing.LastName = updated.LastName;
            existing.Gender = updated.Gender;
            existing.Address = updated.Address;
            existing.EncryptedDateOfBirth = updated.EncryptedDateOfBirth;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _db.Employees.FindAsync(id);
            if (employee == null) return false;

            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
