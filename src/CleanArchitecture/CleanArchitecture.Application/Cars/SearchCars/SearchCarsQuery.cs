using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Cars.SearchCars.Responses;

public sealed record SearchCarsQuery (
    DateOnly Start, DateOnly End
) : IQuery<IReadOnlyList<CarResponse>>;