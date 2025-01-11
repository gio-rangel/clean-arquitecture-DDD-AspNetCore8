using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Cars.Interfaces;
using CleanArchitecture.Domain.Rentals.Entities;
using CleanArchitecture.Domain.Rentals.Errors;
using CleanArchitecture.Domain.Rentals.Interfaces;
using CleanArchitecture.Domain.Rentals.Services;
using CleanArchitecture.Domain.Rentals.ValueObjects;
using CleanArchitecture.Domain.Users.Errors;
using CleanArchitecture.Domain.Users.Interfaces;

namespace CleanArchitecture.Application.Rentals.Book;

internal sealed class BookRentalCommandHandler : ICommandHandler<BookRentalCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly ICarRepository _carRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly RentalPriceService _rentalPriceService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IDateTimeProvider _dateTimeProvider;

    public BookRentalCommandHandler(
        IUserRepository userRepository, 
        ICarRepository carRepository, 
        IRentalRepository rentalRepository, 
        RentalPriceService rentalPriceService, 
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider
    )
    {
        _userRepository = userRepository;
        _carRepository = carRepository;
        _rentalRepository = rentalRepository;
        _rentalPriceService = rentalPriceService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(BookRentalCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if(user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var car = await _carRepository.GetByIdAsync(request.CarId, cancellationToken);

        if(car is null)
        {
            return Result.Failure<Guid>(CarErrors.NotFound);
        }

        var rentalPeriod = DateRange.Create(request.StartDate, request.EndDate);

        if (await _rentalRepository.IsOverlappingAsync(car, rentalPeriod, cancellationToken))
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }

        try {
            var rental = Rental.Book(
                user.Id, 
                car,
                rentalPeriod,
                _dateTimeProvider.currentTime
            ); 

            _rentalRepository.Add(rental);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rental.Id;
        } catch (ConcurrencyException)
        {
            // La excepcion de concurrencia se usa cuando se quiere registrar una entidad que ya fue instanciada 
            return Result.Failure<Guid>(RentalErrors.Overlap); 
        }
    }
}