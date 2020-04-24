using Innovt.Domain.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class SecurityGroupPolicyMap : IEntityTypeConfiguration<SecurityGroupPolicy>
    {
        public void Configure(EntityTypeBuilder<SecurityGroupPolicy> builder)
        {
            builder.ToTable(nameof(SecurityGroupPolicy));

            builder.HasKey(u => u.Id);
            builder.HasOne(p => p.SecurityGroup);
            builder.HasOne(p => p.Policy);
        }
    }
}
