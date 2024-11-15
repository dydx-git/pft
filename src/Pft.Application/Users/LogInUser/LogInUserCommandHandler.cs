using Pft.Application.Abstractions.Authentication;
using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Abstractions;
using Pft.Domain.Entities.Users;

namespace Pft.Application.Users.LogInUser;
internal sealed class LogInUserCommandHandler(IJwtService jwtService)
    : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
    public async Task<Result<AccessTokenResponse>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var result = await jwtService.GetAccessTokenAsync(
            request.Email,
            request.Password, 
            cancellationToken);

        if (result.IsFailure) return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);

        return new AccessTokenResponse(result.Value);
    }
}
