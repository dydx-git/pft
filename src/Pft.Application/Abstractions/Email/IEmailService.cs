namespace Pft.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Pft.Domain.Entities.Users.ValueObjects.Email recipient, string subject, string body);
}
