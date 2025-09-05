using EmployeePayslipApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayslipApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
    }
}
