using Innovt.Domain.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        private readonly bool ignoreCoordinate;
        private readonly bool ignoreCity;
        private readonly bool ignoreType;

        public AddressMap(bool ignoreCoordinate = false, bool ignoreCity = false, bool ignoreType = false)
        {
            this.ignoreCoordinate = ignoreCoordinate;
            this.ignoreCity = ignoreCity;
            this.ignoreType = ignoreType;
        }

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(nameof(Address));

            builder.HasKey(u => u.Id);
            builder.Property(b => b.Description).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.Street).HasMaxLength(150).IsRequired();
            builder.Property(b => b.Number).HasMaxLength(10).IsRequired(false);
            builder.Property(b => b.Complement).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.Neighborhood).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.ZipCode).HasMaxLength(12).IsRequired(false);

            if (ignoreCity)
            {
                builder.Ignore(a => a.City);
                builder.Ignore(a => a.CityId);
            }
            else
            {
                builder.HasOne(a => a.City).WithMany().HasForeignKey(a => a.CityId);
            }


            if (ignoreType)
            {
                builder.Ignore(a => a.Type);
                builder.Ignore(a => a.TypeId);
            }
            else
            {
                builder.HasOne(a => a.Type).WithMany().HasForeignKey(a => a.TypeId);
            }

            if (ignoreCoordinate)
            {
                builder.Ignore(b => b.Coordinate);
            }

            //todo:map coordinates
        }
    }
}