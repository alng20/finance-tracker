using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class ExpenseDetailConfiguration
    : IEntityTypeConfiguration<ExpenseDetail>
{
    public void Configure(EntityTypeBuilder<ExpenseDetail> builder)
    {
        builder.ToTable("ExpenseDetails");

        builder.HasKey(x => x.Id);
        builder
            .HasOne(x => x.Expense)
            .WithMany(x => x.Details)
            .HasForeignKey(x => x.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.ComplexProperty(
            x => x.TotalPrice,
            money =>
            {
                money
                    .Property(x => x.Amount)
                    .HasColumnName("TotalPrice")
                    .HasPrecision(18, 2);
                money
                    .Property(x => x.Currency)
                    .HasColumnName("Currency")
                    .HasConversion<string>();
            }
        );
        builder.ComplexProperty(
            x => x.UnitPrice,
            money =>
            {
                money
                    .Property(x => x.Amount)
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 2);
                money
                    .Property(x => x.Currency)
                    .HasColumnName("UnitPriceCurrency")
                    .HasConversion<string>();
            }
        );
        builder.ComplexProperty(
            x => x.UnitDiscountPrice,
            money =>
            {
                money
                    .Property(x => x.Amount)
                    .HasColumnName("UnitDiscountPrice")
                    .HasPrecision(18, 2);
                money
                    .Property(x => x.Currency)
                    .HasColumnName("UnitDiscountPriceCurrency")
                    .HasConversion<string>();
            }
        );
        builder.Property(x => x.Quantity).HasPrecision(18, 2);
        builder.Property(x => x.DiscountPercent).HasPrecision(5, 2);
    }
}
