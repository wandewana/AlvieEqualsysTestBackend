namespace AlvieEqualsysTestBackend.Models
{
    public class JobPosition
    {
        public int Id { get; set; }
        public string JobName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; } = "Inactive";

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
    }

}
