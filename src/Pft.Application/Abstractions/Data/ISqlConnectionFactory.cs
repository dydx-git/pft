using System.Data;

namespace Pft.Application.Abstractions.Data;
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
