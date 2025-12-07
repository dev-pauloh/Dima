using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<long>> builder)
    {
        builder.ToTable("IdentityUserLogin");
        builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
        builder.Property(ul => ul.LoginProvider).HasMaxLength(128);
        builder.Property(ul => ul.ProviderKey).HasMaxLength(128);
        builder.Property(ul => ul.ProviderDisplayName).HasMaxLength(255);
    }
}