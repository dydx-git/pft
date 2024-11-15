using Microsoft.EntityFrameworkCore;
using Pft.Domain.Entities.Accounts;
using Pft.Domain.Entities.Transactions;

namespace Pft.Infrastructure.Repositories;

internal class AccountRepository(ApplicationDbContext dbContext)
    : Repository<Account, AccountId>(dbContext), IAccountRepository
{
    public async Task<Account?> GetAccountWithTransactionsAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Account>()
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
    }
}

internal class TransactionRepository(ApplicationDbContext dbContext)
    : Repository<Transaction, TransactionId>(dbContext), ITransactionRepository
{
    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Transaction>()
            .Where(t => t.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }
}