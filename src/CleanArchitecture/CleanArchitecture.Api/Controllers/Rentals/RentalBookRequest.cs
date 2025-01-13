namespace CleanArchitecture.Api.Controllers.Cars; 

public sealed record RentalBookRequest (
    Guid CarId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate
);