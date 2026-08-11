using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class RetailerConfiguration : IEntityTypeConfiguration<Retailer>
{
    public void Configure(EntityTypeBuilder<Retailer> builder)
    {
        builder.ToTable("Retailers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200).HasColumnType("citext");
        builder.Property(x => x.DeletedAt).IsRequired(false);
        builder.HasQueryFilter(x => x.DeletedAt == null);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
