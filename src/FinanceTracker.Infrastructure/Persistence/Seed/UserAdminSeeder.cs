using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Options;
using FinanceTracker.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinanceTracker.Infrastructure.Persistence.Seed;

public class UserAdminSeeder
{
    private readonly UserAdminOptions _options;

    public UserAdminSeeder(IOptions<UserAdminOptions> options)
    {
        _options = options.Value;
    }

    public async Task SeedAsync(FinanceTrackerDbContext context)
    {
        bool exists = await context.Users.AnyAsync(x => x.Role == UserRole.Admin);

        if (exists)
        {
            return;
        }

        var user = User.Create(
            _options.FirstName,
            _options.LastName,
            _options.Email,
            UserRole.Admin,
            null
        );

        var passwordHasher = new PasswordHasher<User>();

        user.SetPasswordHash(passwordHasher.HashPassword(user, _options.Password));

        context.Users.Add(user);

        await context.SaveChangesAsync();
    }
}
