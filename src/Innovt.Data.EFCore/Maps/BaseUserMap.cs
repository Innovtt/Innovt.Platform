using Innovt.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class BaseUserMap : IEntityTypeConfiguration<BaseUser>
    {
    
        public void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(b => b.FirstName).HasMaxLength(50).IsRequired(true);
            builder.Property(b => b.LastName).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.Email).HasMaxLength(300).IsRequired(true);
            builder.Property(b => b.Password).HasMaxLength(50).IsRequired(false);
        }
    }
}
