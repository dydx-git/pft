using System.Data;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pft.Application.Abstractions.Clock;
using Pft.Application.Abstractions.Data;
using Pft.Domain.Abstractions;
using Quartz;

namespace Pft.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger)
    : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages");

        using var connection = sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction: transaction);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;

                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Exception while processing outbox message {MessageId}", outboxMessage.Id);

                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        var sql = $"""
            SELECT id, content
            FROM outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY ocurred_on_utc
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
            """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id";

        await connection.ExecuteAsync(sql, new
        {
            outboxMessage.Id,
            ProcessedOnUtc = dateTimeProvider.UtcNow,
            Error = exception?.ToString()
        },
        transaction: transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
