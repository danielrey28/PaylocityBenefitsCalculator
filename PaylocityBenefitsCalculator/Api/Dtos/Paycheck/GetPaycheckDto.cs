using Api.Models;

namespace Api.Dtos.Paycheck;

//I added some fields that would make sense for a paycheck in addition to the required Gross and Net pay fields.
public class GetPaycheckDto
{
    public int EmployeeId { get; set; } 
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public decimal TotalDeductions { get; set; }
    public ICollection<Deduction> Deductions { get; set; } = new List<Deduction>();
}