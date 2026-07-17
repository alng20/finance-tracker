using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");

        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SharedGroup).WithMany().HasForeignKey(x => x.SharedGroupId);
        builder.HasOne(x => x.Shop).WithMany().HasForeignKey(x => x.ShopId);
        builder.HasMany(x => x.Details).WithOne(x => x.Expense).HasForeignKey(x => x.ExpenseId);
        builder.OwnsOne(
            x => x.TotalAmount,
            money =>
            {
                money.Property(x => x.Amount).HasColumnName("TotalAmount").HasPrecision(18, 2);
                money.Property(x => x.Currency).HasColumnName("Currency").HasConversion<string>();
            }
        );
        builder.Property(x => x.Date).IsRequired();
    }
}
