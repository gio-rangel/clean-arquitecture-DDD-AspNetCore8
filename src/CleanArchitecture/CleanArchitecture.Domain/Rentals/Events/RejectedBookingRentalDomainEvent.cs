using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Events;

public sealed record RejectedBookingRentalDomainEvent(Guid RentalId) : IDomainEvent;