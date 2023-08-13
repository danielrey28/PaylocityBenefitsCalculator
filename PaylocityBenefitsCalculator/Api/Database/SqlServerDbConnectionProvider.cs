using System.Data;
using System.Data.SqlClient;

namespace Api.Database;

public class SqlServerDbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public SqlServerDbConnectionProvider(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlConnection");
    }

    public IDbConnection Connect()
    {
        return new SqlConnection(_connectionString);
    }
}