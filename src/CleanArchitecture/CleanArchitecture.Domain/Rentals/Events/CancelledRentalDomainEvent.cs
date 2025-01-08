using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Events;

public sealed record CancelRentalDomainEvent(Guid RentalId) : IDomainEvent;