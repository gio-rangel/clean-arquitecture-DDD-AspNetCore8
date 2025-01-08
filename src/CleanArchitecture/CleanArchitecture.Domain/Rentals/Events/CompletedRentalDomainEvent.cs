using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Events;

public sealed record CompletedRentalDomainEvent(Guid RentalId) : IDomainEvent;