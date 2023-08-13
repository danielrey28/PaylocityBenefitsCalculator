using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Helpers;
using Api.Models;
using Api.Repositories;

namespace Api.Services
{
    public interface IPaycheckService
    {
        Task<GetPaycheckDto?> GetPaycheck(int employeeId);
    }

    public class PaycheckService : IPaycheckService
    {
        private readonly IEmployeesRepository _employeesRepository;

        public PaycheckService(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<GetPaycheckDto?> GetPaycheck(int employeeId)
        {
            var employee = await _employeesRepository.GetAsync(employeeId);

            if (employee == null)
            {
                return null;
            }

            var grossPay = Math.Round(employee.Salary / 26m, 2);
            var deductions = new List<Deduction>
            {
                new()
                {
                    DeductionAmount =  Math.Round(1000 * 12 / 26m, 2), //Ideally, deduction amounts would come from a Deductions table that's related to an employee or dependent 
                    DeductionName = "Employee Deduction"
                }
            };

            AddDependentDeductions(employee, deductions);
            AddHighEarnerDeduction(employee, deductions);
            AddAgeBasedDeduction(employee, deductions);

            var totalDeductions = deductions.Sum(d => d.DeductionAmount);
            var netPay = Math.Round(grossPay - totalDeductions, 2);

            return new GetPaycheckDto
            {
                EmployeeId = employeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                GrossPay = grossPay,
                NetPay = netPay,
                TotalDeductions = totalDeductions,
                Deductions = deductions
            };

        }

        private static void AddDependentDeductions(GetEmployeeDto employee, ICollection<Deduction> deductions)
        {
            var dependentDeductionPerPaycheck = Math.Round(600 * 12 / 26m, 2); 

            foreach (var dependent in employee.Dependents)
            {
                deductions.Add(new Deduction
                {
                    DeductionName = $"{dependent.Relationship.GetDisplayName()} Deduction",
                    DeductionAmount = dependentDeductionPerPaycheck
                });
            }
        }

        private static void AddAgeBasedDeduction(GetEmployeeDto employee, ICollection<Deduction> deductions)
        {
            var employeeAge = DateTime.Now.Year - employee.DateOfBirth.Year;

            if (DateTime.Now.DayOfYear < employee.DateOfBirth.DayOfYear)
            {
                employeeAge--;
            }

            if (employeeAge >= 50)
            {
                deductions.Add(new Deduction
                {
                    DeductionAmount = 200m,
                    DeductionName = "Senior Deduction"
                });
            }
        }

        private static void AddHighEarnerDeduction(GetEmployeeDto employee, ICollection<Deduction> deductions)
        {
            if (employee.Salary >= 80000m)
            {
                var higherEarnerDeduction = Math.Round(employee.Salary * 0.02m / 26m, 2);
                deductions.Add(new Deduction()
                {
                    DeductionAmount = higherEarnerDeduction,
                    DeductionName = "High Earner Deduction"
                });
            }
        }
    }
}
