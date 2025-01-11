using CleanArchitecture.Domain.Reviews.Interfaces;
using CleanArchitecture.Domain.Reviews.Entities;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
