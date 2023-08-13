using Api.Database;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Dapper;

namespace Api.Repositories
{
    public interface IEmployeesRepository
    {
        Task<IEnumerable<GetEmployeeDto>> GetAllAsync();
        Task<GetEmployeeDto?> GetAsync(int id);
    }

    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public EmployeesRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<GetEmployeeDto?> GetAsync(int id)
        {
            var sql = @"SELECT e.Id, 
                            e.FirstName,
                            e.LastName, 
                            e.Salary,
                            e.DateOfBirth,
                            d.Id,
                            d.FirstName,
                            d.LastName,
                            d.DateOfBirth,
                            d.Relationship
                        FROM Employee e
                        FULL JOIN Dependent d on d.EmployeeId =  e.Id
                        WHERE e.Id=@id"; //parameterized sql to prevent sql injection attacks

            var employeeDictionary = new Dictionary<int, GetEmployeeDto>();

            using (var connection = _dbConnectionProvider.Connect())
            {
                var employees = await connection.QueryAsync<GetEmployeeDto, GetDependentDto, GetEmployeeDto>(sql,
                    (employee, dependent) => GetEmployeeData(employeeDictionary, employee, dependent), new { id }, splitOn:"Id");
                return employees.FirstOrDefault();
            }
        }
        
        public async Task<IEnumerable<GetEmployeeDto>> GetAllAsync()
        {
            const string sql = @"SELECT e.Id, 
                                    e.FirstName, 
                                    e.LastName, 
                                    e.Salary, 
                                    e.DateOfBirth,
                                    d.Id,
                                    d.FirstName,
                                    d.LastName,
                                    d.DateOfBirth,
                                    d.Relationship
                                FROM Employee e 
                                FULL JOIN Dependent d on d.EmployeeId = e.Id";

            var employeeDictionary = new Dictionary<int, GetEmployeeDto>();

            using (var connection = _dbConnectionProvider.Connect())
            {
               var employees = await connection.QueryAsync<GetEmployeeDto, GetDependentDto, GetEmployeeDto>(sql,
                    (employee, dependent) => GetEmployeeData(employeeDictionary, employee, dependent), splitOn: "Id");
                return employees.Distinct();
            }
        }
        
        // This method gets all of the employees from the sql result and assigns the related dependents to each employee.
        private static GetEmployeeDto GetEmployeeData(Dictionary<int, GetEmployeeDto> employeeDictionary, GetEmployeeDto employee,
            GetDependentDto dependent)
        {
            if (!employeeDictionary.TryGetValue(employee.Id, out var emp))
            {
                emp = employee;
                emp.Dependents = new List<GetDependentDto>();
                employeeDictionary.Add(employee.Id, emp);
            }

            if (dependent != null && dependent.Id > 0)
            {
                emp.Dependents.Add(dependent);
            }

            return emp;
        }
    }
}
