namespace Pft.Domain.Entities.Accounts;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(AccountId id, CancellationToken cancellationToken = default);
    void Add(Account entity);
    Task<Account?> GetAccountWithTransactionsAsync(AccountId accountId, CancellationToken cancellationToken = default);
}