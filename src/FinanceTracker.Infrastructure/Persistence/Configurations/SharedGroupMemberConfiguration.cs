using FinanceTracker.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class SharedGroupMemberConfiguration
    : IEntityTypeConfiguration<SharedGroupMember>
{
    public void Configure(EntityTypeBuilder<SharedGroupMember> builder)
    {
        builder.ToTable("SharedGroupMembers");

        builder.HasKey(x => new { x.SharedGroupId, x.UserId });
        builder.Property(x => x.Permission).HasConversion<int>();
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.GroupMemberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(x => x.SharedGroup)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.SharedGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.JoinedAt).IsRequired();
        builder.Property(x => x.DeletedAt).IsRequired(false);
        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}
