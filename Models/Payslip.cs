using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePayslipApp.Models
{
    public class Payslip
    {
        public int Id { get; set; }

        public double HRA { get; set; }
        public double DA { get; set; }
        public double PF { get; set; }
        public double Tax { get; set; }
        public double GrossSalary { get; set; }
        public double Deductions { get; set; }
        public double NetSalary { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}
