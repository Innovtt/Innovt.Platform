using Innovt.Domain.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class SecurityGroupMap : IEntityTypeConfiguration<SecurityGroup>
    {
        public void Configure(EntityTypeBuilder<SecurityGroup> builder)
        {
            builder.ToTable(nameof(SecurityGroup));

            builder.HasKey(u => u.Id);
            builder.Property(b => b.Name).HasMaxLength(50).IsRequired(true);
            builder.Property(b => b.Description).HasMaxLength(100).IsRequired(false);
            builder.HasMany(b => b.Users);
            builder.HasMany(b => b.Policies);

        }
    }
}
