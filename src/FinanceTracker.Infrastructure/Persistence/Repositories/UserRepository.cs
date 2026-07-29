using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Interfaces.Persistence;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FinanceTrackerDbContext _ctx;

    public UserRepository(FinanceTrackerDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _ctx.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
    
    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        User? user = await _ctx.Users.FindAsync(new object[] { id }, cancellationToken);
        return user ?? throw new NotFoundException($"User was not found");
    }

    public void Add(User user)
    {
        _ctx.Users.Add(user);
    }
}
