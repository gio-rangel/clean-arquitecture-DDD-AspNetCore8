using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Events;

public sealed record BookedRentalDomainEvent(Guid RentalId) : IDomainEvent;