using AlvieEqualsysTestBackend.Data;
using Microsoft.EntityFrameworkCore;
using AlvieEqualsysTestBackend.Models;
using AlvieEqualsysTestBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=employee.db"));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IJobPositionService, JobPositionService>();

var app = builder.Build();

// Seed dummy data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Ensures DB exists, doesn't apply migrations

    if (!db.Employees.Any())
    {
        var employee = new Employee
        {
            FirstName = "John",
            MiddleName = "A",
            LastName = "Doe",
            Gender = "Male",
            Address = "123 Main St",
            EncryptedDateOfBirth = Encrypt("1988-05-23"),
            JobPositions = new List<JobPosition>
            {
                new JobPosition
                {
                    JobName = "Software Engineer",
                    StartDate = DateTime.UtcNow.AddYears(-2),
                    Salary = 90000,
                    Status = "Active"
                }
            }
        };

        db.Employees.Add(employee);
        db.SaveChanges();
        Console.WriteLine("Dummy employee seeded.");
    }
    else
    {
        Console.WriteLine("Employee data already exists.");
    }
}

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


// 🔐 Simple base64 encryption for DOB (you can switch to AES later)
string Encrypt(string plainText)
{
    var bytes = System.Text.Encoding.UTF8.GetBytes(plainText);
    return Convert.ToBase64String(bytes);
}
