using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Paycheck;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public class PaycheckIntegrationTests : IntegrationTest
    {
        [Fact]

        public async Task WhenAskedForEmployeePaycheck_ShouldReturnEmployeeNoDependentsPaycheck()
        {
            var response = await HttpClient.GetAsync("/api/v1/paycheck/1");
            var expectedPaycheck = new GetPaycheckDto
            {
                EmployeeId = 1,
                FirstName = "LeBron",
                LastName = "James",
                GrossPay = 2900.81m,
                NetPay = 2439.27m,
                TotalDeductions = 461.54m,
                Deductions = new List<Deduction>
                {
                    new Deduction()
                    {
                        DeductionAmount = 461.54m,
                        DeductionName = "Employee Deduction"
                    }
                }
            };

            await response.ShouldReturn(HttpStatusCode.OK, expectedPaycheck);
        }

        [Fact]
        public async Task WhenAskedForEmployeePaycheck_ShouldReturnEmployeeWithDependentsPaycheck()
        {
            var response = await HttpClient.GetAsync("/api/v1/paycheck/2");
            var expectedPaycheck = new GetPaycheckDto
            {
                EmployeeId = 2,
                FirstName = "Ja",
                LastName = "Morant",
                GrossPay = 3552.51m,
                NetPay = 2189.16m,
                TotalDeductions = 1363.35m,
                Deductions = new List<Deduction>
                {
                    new Deduction()
                    {
                        DeductionAmount = 461.54m,
                        DeductionName = "Employee Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 276.92m,
                        DeductionName = "Spouse Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 276.92m,
                        DeductionName = "Child Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 276.92m,
                        DeductionName = "Child Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 71.05m,
                        DeductionName = "High Earner Deduction"
                    }
                }
            };

            await response.ShouldReturn(HttpStatusCode.OK, expectedPaycheck);
        }

        [Fact]
        public async Task WhenAskedForEmployeePaycheck_ShouldReturnSeniorEmployeeWithDependentsPaycheck()
        {
            var response = await HttpClient.GetAsync("/api/v1/paycheck/3");
            var expectedPaycheck = new GetPaycheckDto
            {
                EmployeeId = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                GrossPay = 5508.12m,
                NetPay = 4459.50m,
                TotalDeductions = 1048.62m,
                Deductions = new List<Deduction>
                {
                    new Deduction()
                    {
                        DeductionAmount = 461.54m,
                        DeductionName = "Employee Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 276.92m,
                        DeductionName = "Domestic Partner Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 110.16m,
                        DeductionName = "High Earner Deduction"
                    },
                    new Deduction()
                    {
                        DeductionAmount = 200.0m,
                        DeductionName = "Senior Deduction"
                    }
                }
            };

            await response.ShouldReturn(HttpStatusCode.OK, expectedPaycheck);
        }

        [Fact]
        public async Task WhenAskedForANonexistentEmployeePaycheck_ShouldReturn404()
        {
            var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
            await response.ShouldReturn(HttpStatusCode.NotFound);
        }
    }
}
