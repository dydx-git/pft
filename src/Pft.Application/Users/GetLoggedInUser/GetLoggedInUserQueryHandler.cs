using Dapper;
using Pft.Application.Abstractions.Authentication;
using Pft.Application.Abstractions.Data;
using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Abstractions;

namespace Pft.Application.Users.GetLoggedInUser;
internal sealed class GetLoggedInUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id AS Id,
                first_name AS FirstName,
                last_name AS LastName,
                email AS Email
            FROM users
            WHERE identity_id = @IdentityId
            """;

        return await connection.QuerySingleOrDefaultAsync<UserResponse>(
            sql,
            new
            {
                userContext.IdentityId
            });        
    }
}
