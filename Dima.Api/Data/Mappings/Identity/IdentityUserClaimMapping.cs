using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserClaimMapping : IEntityTypeConfiguration<IdentityUserClaim<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<long>> builder)
    {
        builder.ToTable("IdentityUserClaim");
        builder.HasKey(uc => uc.Id);
        builder.Property(uc => uc.ClaimType).HasMaxLength(255);
        builder.Property(uc => uc.ClaimValue).HasMaxLength(255);
    }
}