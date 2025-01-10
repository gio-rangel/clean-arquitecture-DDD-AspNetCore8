using FluentValidation;

namespace CleanArchitecture.Application.Rentals.Book;

public class BookRentalCommandValidator : AbstractValidator<BookRentalCommand>
{
    public BookRentalCommandValidator()
    {
        // Lambda function for applaying conditions in this case validations
        RuleFor(c => c.UserId).NotEmpty(); 
        RuleFor(c => c.CarId).NotEmpty(); 
        RuleFor(c => c.StartDate).LessThan(c => c.EndDate); 
    }
}