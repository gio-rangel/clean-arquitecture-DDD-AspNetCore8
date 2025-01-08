using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Events;

public sealed record ConfirmedRentalDomainEvent(Guid RentalId) : IDomainEvent;