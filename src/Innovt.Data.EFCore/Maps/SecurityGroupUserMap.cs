using Innovt.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class SecurityGroupUserMap : IEntityTypeConfiguration<SecurityGroupUser>
    {
        public void Configure(EntityTypeBuilder<SecurityGroupUser> builder)
        {
            builder.ToTable(nameof(SecurityGroupUser));

            builder.HasKey(u => u.Id);
            builder.HasOne(p => p.SecurityGroup);
            //builder.HasOne(p => p.User);
        }
    }
}
