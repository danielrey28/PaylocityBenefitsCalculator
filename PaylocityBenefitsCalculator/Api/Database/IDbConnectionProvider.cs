using System.Data;

namespace Api.Database;

//This interface will allow other types of relational database connections from being injected into the repository
public interface IDbConnectionProvider
{
    IDbConnection Connect();
}