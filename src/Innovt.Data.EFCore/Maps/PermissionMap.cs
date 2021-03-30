using Innovt.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class PermissionMap : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission));

            builder.HasKey(u => u.Id);
            builder.Property(b => b.Domain).HasMaxLength(30).IsRequired(true);
            builder.Property(b => b.Name).HasMaxLength(50).IsRequired(true);
            builder.Property(b => b.Resource).HasMaxLength(300).IsRequired(true);
        }
    }
}