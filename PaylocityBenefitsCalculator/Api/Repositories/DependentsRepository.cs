using Api.Database;
using Api.Dtos.Dependent;
using Dapper;

namespace Api.Repositories;

public interface IDependentsRepository
{
    Task<GetDependentDto?> GetAsync(int id);
    Task<IEnumerable<GetDependentDto>> GetAllAsync();
}

public class DependentsRepository : IDependentsRepository
{
    private readonly IDbConnectionProvider _dbConnectionProvider;

    public DependentsRepository(IDbConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public async Task<GetDependentDto?> GetAsync(int id)
    {
        const string sql = @"SELECT 
                                Id,
                                FirstName,
                                LastName,
                                DateOfBirth,
                                Relationship
                            FROM Dependent
                            WHERE Id=@id"; //parameterized sql to prevent sql injection attacks

        using (var connection = _dbConnectionProvider.Connect())
        {
            IEnumerable<GetDependentDto?> dependents = await connection.QueryAsync<GetDependentDto>(sql, new {id});
            return dependents.FirstOrDefault();
        }
    }

    public async Task<IEnumerable<GetDependentDto>> GetAllAsync()
    {

        const string sql = @"SELECT 
                                Id,
                                FirstName,
                                LastName,
                                DateOfBirth,
                                Relationship
                            FROM Dependent";

        using (var connection = _dbConnectionProvider.Connect())
        {
            return await connection.QueryAsync<GetDependentDto>(sql);
        }
    }
}
