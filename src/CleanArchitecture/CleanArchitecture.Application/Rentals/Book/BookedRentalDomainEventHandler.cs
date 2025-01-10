using CleanArchitecture.Application.Abstractions.Emails;
using CleanArchitecture.Domain.Rentals.Events;
using CleanArchitecture.Domain.Rentals.Interfaces;
using CleanArchitecture.Domain.Users.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Rentals.Book;

internal sealed class BookedRentalDomainEventHandler
: INotificationHandler<BookedRentalDomainEvent>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public BookedRentalDomainEventHandler(
        IRentalRepository rentalRepository, 
        IUserRepository userRepository, 
        IEmailService emailService
    )
    {
        _rentalRepository = rentalRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(
        BookedRentalDomainEvent notification, 
        CancellationToken cancellationToken
    )
    {
        var rental = await _rentalRepository.GetRentalByIdAsync(notification.RentalId, cancellationToken);

        if(rental is null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(rental.UserId, cancellationToken);

        if(user is null)
        {
            return;
        }

        await _emailService.SendAsync(user.Email!, "Car Rental Booked successfully", "Go to the page to confirm the reservation before init day or it will be cancelled.");
    }

    public Task Handle(BookedRentalDomainEventHandler notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}