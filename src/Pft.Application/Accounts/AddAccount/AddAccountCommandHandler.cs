using Pft.Application.Abstractions.Clock;
using Pft.Application.Abstractions.Messaging;
using Pft.Domain.Abstractions;
using Pft.Domain.Entities.Accounts;
using Pft.Domain.Entities.Users;

namespace Pft.Application.Accounts.AddAccount;

public sealed class AddAccountCommandHandler(
    IUserRepository userRepository,
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork, 
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddAccountCommand, Guid>
{
    public Task<Result<Guid>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}