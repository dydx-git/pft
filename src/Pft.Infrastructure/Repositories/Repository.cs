﻿using Microsoft.EntityFrameworkCore;
using Pft.Domain.Entities;

namespace Pft.Infrastructure.Repositories;
internal abstract class Repository<TEntity, TEntityId>(ApplicationDbContext dbContext)
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public virtual void Add(TEntity entity) => DbContext.Add(entity);
}
