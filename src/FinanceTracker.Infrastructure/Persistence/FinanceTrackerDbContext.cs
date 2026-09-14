using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

public class FinanceTrackerDbContext : DbContext
{
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseDetail> ExpenseDetails => Set<ExpenseDetail>();

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<SharedGroup> SharedGroups => Set<SharedGroup>();
    public DbSet<SharedGroupMember> SharedGroupMembers => Set<SharedGroupMember>();

    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Retailer> Retailers => Set<Retailer>();

    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();

    public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceTrackerDbContext).Assembly);
    }
}
