using Pft.Application.Abstractions.Messaging;

namespace Pft.Application.Users.GetLoggedInUser;
public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
