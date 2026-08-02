using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.ToTable("Shops");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder
            .HasOne(x => x.Retailer)
            .WithMany()
            .HasForeignKey(x => x.RetailerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.ComplexProperty(
            x => x.Address,
            address =>
            {
                address.Property(x => x.Country).HasColumnName("Country").HasConversion<string>();
                address.Property(x => x.City).HasColumnName("City").HasConversion<string>();
            }
        );
        builder.Property(x => x.DeletedAt).IsRequired(false);
        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}
