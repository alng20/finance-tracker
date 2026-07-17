using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

public class FinanceTrackerDbContext : DbContext
{
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseDetail> ExpenseDetails => Set<ExpenseDetail>();

    public DbSet<User> Users => Set<User>();
    public DbSet<Retailer> Retailers => Set<Retailer>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Item> Items => Set<Item>();

    public DbSet<SharedGroup> SharedGroups => Set<SharedGroup>();
    public DbSet<SharedGroupMember> SharedGroupMembers => Set<SharedGroupMember>();

    public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceTrackerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
