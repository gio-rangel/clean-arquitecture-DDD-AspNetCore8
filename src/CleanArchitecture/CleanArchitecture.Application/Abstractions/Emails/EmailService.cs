using CleanArchitecture.Domain.Users.ValueObjects;

namespace CleanArchitecture.Application.Abstractions.Emails;

public interface IEmailService
{
    Task SendAsync(Email recipient, string subject, string body); 
}