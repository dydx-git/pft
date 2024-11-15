using Dapper;
using Pft.Application.Abstractions.Data;
using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Abstractions;

namespace Pft.Application.Accounts.GetAccount;

public class GetAccountQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetAccountQuery, AccountResponse>
{
    public async Task<Result<AccountResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();
        
        const string sql = """
            SELECT
                id AS Id,
                email AS Email,
                first_name AS FirstName,
                last_name AS LastName,
                created_on_utc AS CreatedOnUtc
            FROM accounts
            WHERE id = @AccountId
            """;
        
        var account = await connection.QueryFirstOrDefaultAsync<AccountResponse>(
            sql,
            new
            {
                request.AccountId
            });
        
        return account;
    }
}