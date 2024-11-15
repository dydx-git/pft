using System.Data;
using Npgsql;
using Pft.Application.Abstractions.Data;

namespace Pft.Infrastructure.Data;
internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}
