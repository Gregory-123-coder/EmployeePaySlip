using System.ComponentModel.DataAnnotations;

namespace EmployeePayslipApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Department { get; set; }

        [Range(1000, 1000000)]
        public double BasicSalary { get; set; }

        public Payslip Payslip { get; set; } = new Payslip();
    }
}
