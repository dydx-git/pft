using Pft.Application.Abstractions.Messaging;

namespace Pft.Application.Users.LogInUser;
public sealed record LogInUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;