using Pft.Application.Abstractions.Email;

namespace Pft.Infrastructure.Email;
internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Entities.Users.ValueObjects.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
