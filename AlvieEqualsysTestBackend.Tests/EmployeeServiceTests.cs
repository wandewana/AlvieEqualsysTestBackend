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
    public class EmployeeServiceTests
    {
        private AppDbContext GetDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(name)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Valid_Employee()
        {
            var db = GetDbContext("CreateEmployee");
            var service = new EmployeeService(db);

            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "Test Street",
                Gender = "Male",
                EncryptedDateOfBirth = Convert.ToBase64String(Encoding.UTF8.GetBytes("1990-01-01"))
            };

            var result = await service.CreateAsync(employee);

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task UpdateAsync_Should_Change_Fields()
        {
            var db = GetDbContext("UpdateEmployee");
            var service = new EmployeeService(db);

            var employee = new Employee { FirstName = "Anna", LastName = "Lee", Gender = "Female", Address = "Old", EncryptedDateOfBirth = "enc" };
            db.Employees.Add(employee);
            db.SaveChanges();

            employee.Address = "New";
            var updated = await service.UpdateAsync(employee.Id, employee);

            Assert.Equal("New", updated?.Address);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Employee()
        {
            var db = GetDbContext("DeleteEmployee");
            var service = new EmployeeService(db);

            var e = new Employee { FirstName = "Del", LastName = "Test", Gender = "Other", Address = "", EncryptedDateOfBirth = "enc" };
            db.Employees.Add(e);
            db.SaveChanges();

            var result = await service.DeleteAsync(e.Id);

            Assert.True(result);
            Assert.Empty(db.Employees.ToList());
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Null_For_Unknown_Id()
        {
            var db = GetDbContext("UnknownId");
            var service = new EmployeeService(db);

            var employee = await service.GetByIdAsync(999);

            Assert.Null(employee);
        }
    }
}
