using CleanArchitecture.Application.Abstractions.Emails;

namespace CleanArchitecture.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(
        Domain.Users.ValueObjects.Email recipient, 
        string subject, 
        string body
    )
    {
        return Task.CompletedTask; 
    }
}