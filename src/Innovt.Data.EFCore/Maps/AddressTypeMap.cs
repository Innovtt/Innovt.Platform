using Innovt.Domain.Model.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class AddressTypeMap : IEntityTypeConfiguration<AddressType>
    {
        public void Configure(EntityTypeBuilder<AddressType> builder)
        {
            builder.ToTable(nameof(AddressType));

            builder.HasKey(u => u.Id);
            builder.Property(b => b.Name).HasMaxLength(15).IsRequired();
        }
    }
}
