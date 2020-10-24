using Innovt.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class PolicyPermissionMap : IEntityTypeConfiguration<PolicyPermission>
    {
        public void Configure(EntityTypeBuilder<PolicyPermission> builder)
        {
            builder.ToTable(nameof(PolicyPermission));

            builder.HasKey(u => u.Id);
            builder.HasOne(p => p.Permission);
            builder.HasOne(p => p.Policy);
        }
    }
}
