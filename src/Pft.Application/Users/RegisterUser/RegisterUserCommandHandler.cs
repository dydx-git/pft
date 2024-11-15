using Pft.Application.Abstractions.Authentication;
using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Abstractions;
using Pft.Domain.Entities.Users;
using Pft.Domain.Entities.Users.ValueObjects;

namespace Pft.Application.Users.RegisterUser;
internal sealed class RegisterUserCommandHandler(
    IAuthenticationService authenticationService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.FirstName,
            request.LastName,
            new Email(request.Email));

        var identityId = await authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityId);

        userRepository.Add(user);

        await unitOfWork.SaveChangesAsync();

        return user.Id.Value;
    }
}
