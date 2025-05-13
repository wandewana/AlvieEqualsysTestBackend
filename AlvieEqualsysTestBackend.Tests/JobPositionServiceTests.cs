using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlvieEqualsysTestBackend.Data;
using AlvieEqualsysTestBackend.Models;
using AlvieEqualsysTestBackend.Services;
using Microsoft.EntityFrameworkCore;

namespace AlvieEqualsysTestBackend.Tests
{
    public class JobPositionServiceTests
    {
        private AppDbContext GetDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(name)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddToEmployeeAsync_Should_Add_Job()
        {
            var db = GetDbContext("AddJob");
            var employee = new Employee { FirstName = "Sam", LastName = "Smith", Gender = "M", Address = "", EncryptedDateOfBirth = "enc" };
            db.Employees.Add(employee);
            db.SaveChanges();

            var service = new JobPositionService(db);
            var job = new JobPosition { JobName = "Engineer", StartDate = DateTime.Today, Status = "Active", Salary = 60000 };

            var added = await service.AddToEmployeeAsync(employee.Id, job);

            Assert.Equal("Engineer", added.JobName);
            Assert.Equal(employee.Id, added.EmployeeId);
        }

        [Fact]
        public async Task AddToEmployeeAsync_Should_Throw_If_Active_Exists()
        {
            var db = GetDbContext("ActiveConflict");
            var employee = new Employee { FirstName = "Conflict", LastName = "Test", Gender = "M", Address = "", EncryptedDateOfBirth = "enc" };
            db.Employees.Add(employee);
            db.JobPositions.Add(new JobPosition { JobName = "A", Status = "Active", StartDate = DateTime.Today, Employee = employee });
            db.SaveChanges();

            var service = new JobPositionService(db);
            var job = new JobPosition { JobName = "B", Status = "Active", StartDate = DateTime.Today };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AddToEmployeeAsync(employee.Id, job));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Salary()
        {
            var db = GetDbContext("UpdateJob");
            var e = new Employee { FirstName = "Pay", LastName = "Test", Gender = "F", Address = "", EncryptedDateOfBirth = "enc" };
            var job = new JobPosition { JobName = "Dev", StartDate = DateTime.Today, Status = "Inactive", Salary = 50000, Employee = e };
            db.Employees.Add(e);
            db.JobPositions.Add(job);
            db.SaveChanges();

            var service = new JobPositionService(db);
            job.Salary = 65000;

            var updated = await service.UpdateAsync(job.Id, job);

            Assert.Equal(65000, updated?.Salary);
        }

        [Fact]
        public async Task GetByEmployeeIdAsync_Returns_Jobs()
        {
            var db = GetDbContext("ListJobs");
            var e = new Employee { FirstName = "Jobs", LastName = "Test", Gender = "F", Address = "", EncryptedDateOfBirth = "enc" };
            db.Employees.Add(e);
            db.JobPositions.AddRange(
                new JobPosition { JobName = "Dev", Status = "Inactive", StartDate = DateTime.Today, Employee = e },
                new JobPosition { JobName = "QA", Status = "Inactive", StartDate = DateTime.Today, Employee = e }
            );
            db.SaveChanges();

            var service = new JobPositionService(db);
            var jobs = await service.GetByEmployeeIdAsync(e.Id);

            Assert.Equal(2, jobs.Count);
        }
    }
}
