using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users.Errors;

public static class UserErrors 
{
    public static Error NotFound = new Error("User.NotFound", "User with provided id not found.");
}