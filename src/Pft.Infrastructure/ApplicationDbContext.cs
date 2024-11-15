using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pft.Application.Abstractions.Clock;
using Pft.Application.Exceptions;
using Pft.Domain.Abstractions;
using Pft.Infrastructure.Outbox;

namespace Pft.Infrastructure;
public sealed class ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider)
    : DbContext(options), IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    //private readonly IPublisher _publisher;

    //IPublisher publisher,
    //_publisher = publisher;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            //First process the domain events and adding them to ChangeTracker as Outbox Messages,
            //then persisting everything in the database in a single transaction "atomic operation" 
            AddDomainEventsAsOutboxMessages();

            var result = await base.SaveChangesAsync(cancellationToken);
            
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception ocurred.", ex);
        }
        
    }

    //Publishing an event can fail and is not in a transaction. Changing to Outbox pattern
    //private async Task PublishDomainEventsAsync()
    //{
    //    /*
    //     Using the ChangeTracker, we grab the entries which implement the Entity class. 
    //     Then we grab all the domain events and publish one by one.         
    //     */
    //    var domainEvents = ChangeTracker
    //        .Entries<IEntity>()
    //        .Select(entry => entry.Entity)
    //        .SelectMany(entity =>
    //        {
    //            var domainEvents = entity.GetDomainEvents();

    //            entity.ClearDomainEvents();

    //            return domainEvents;
    //        }).ToList();

    //    foreach (var domainEvent in domainEvents) await _publisher.Publish(domainEvent);
    //}

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();

        AddRange(outboxMessages);
    }
}
