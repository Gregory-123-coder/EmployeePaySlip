using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeePayslipApp.Data;
using EmployeePayslipApp.Models;

namespace EmployeePayslipApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(e => e.Payslip).ToListAsync();
            return View(employees);
        }

        // GET: /Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Calculate payslip
                    var hra = employee.BasicSalary * 0.20;
                    var da = employee.BasicSalary * 0.10;
                    var pf = employee.BasicSalary * 0.12;
                    var tax = employee.BasicSalary * 0.15;

                    var gross = employee.BasicSalary + hra + da;
                    var deductions = pf + tax;
                    var net = gross - deductions;

                    var payslip = new Payslip
                    {
                        HRA = hra,
                        DA = da,
                        PF = pf,
                        Tax = tax,
                        GrossSalary = gross,
                        Deductions = deductions,
                        NetSalary = net
                    };

                    employee.Payslip = payslip;

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving employee: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the employee.");
                }
            }

            return View(employee);
        }

        // GET: /Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                                         .Include(e => e.Payslip)
                                         .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: /Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var hra = employee.BasicSalary * 0.20;
                    var da = employee.BasicSalary * 0.10;
                    var pf = employee.BasicSalary * 0.12;
                    var tax = employee.BasicSalary * 0.15;

                    var gross = employee.BasicSalary + hra + da;
                    var deductions = pf + tax;
                    var net = gross - deductions;

                    var existingEmployee = await _context.Employees
                                                         .Include(e => e.Payslip)
                                                         .FirstOrDefaultAsync(e => e.Id == id);

                    if (existingEmployee == null)
                        return NotFound();

                    // Update employee data
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Department = employee.Department;
                    existingEmployee.BasicSalary = employee.BasicSalary;

                    // Update payslip
                    existingEmployee.Payslip.HRA = hra;
                    existingEmployee.Payslip.DA = da;
                    existingEmployee.Payslip.PF = pf;
                    existingEmployee.Payslip.Tax = tax;
                    existingEmployee.Payslip.GrossSalary = gross;
                    existingEmployee.Payslip.Deductions = deductions;
                    existingEmployee.Payslip.NetSalary = net;

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.Id == employee.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(employee);
        }
        // GET: Employee/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null || _context.Employees == null)
        return NotFound();

    var employee = await _context.Employees
        .Include(e => e.Payslip)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (employee == null)
        return NotFound();

    return View(employee);
}

// POST: Employee/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var employee = await _context.Employees.Include(e => e.Payslip).FirstOrDefaultAsync(e => e.Id == id);
    if (employee != null)
    {
        _context.Payslips.Remove(employee.Payslip);
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

    }
}
