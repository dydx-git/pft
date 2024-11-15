using Pft.Domain.Abstractions;

namespace Pft.Application.Abstractions.Authentication;
public interface IJwtService
{
    Task<Result<string>> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
