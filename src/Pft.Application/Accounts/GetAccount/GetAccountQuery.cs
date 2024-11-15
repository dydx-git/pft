using Pft.Application.Abstractions.Caching;

namespace Pft.Application.Accounts.GetAccount;

public record GetAccountQuery(Guid AccountId) : ICachedQuery<AccountResponse>
{
    public string CacheKey => $"accounts-{AccountId}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(1);
}