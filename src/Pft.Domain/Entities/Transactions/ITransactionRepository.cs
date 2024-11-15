using Pft.Domain.Entities.Accounts;

namespace Pft.Domain.Entities.Transactions;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(TransactionId id, CancellationToken cancellationToken = default);
    void Add(Transaction entity);
    Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default);
}