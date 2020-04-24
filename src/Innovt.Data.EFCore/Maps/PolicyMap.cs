
using Innovt.Domain.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class PolicyMap : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable(nameof(Policy));

            builder.HasKey(u => u.Id);
            builder.Property(b => b.Name).HasMaxLength(50).IsRequired(true);
            builder.Property(b => b.Description).HasMaxLength(100).IsRequired(false);
        }
    }
}
