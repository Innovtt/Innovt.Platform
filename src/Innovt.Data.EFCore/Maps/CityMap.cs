using Innovt.Domain.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable(nameof(City));

            builder.HasKey(c => c.Id);

            builder.Property(d => d.Name).HasMaxLength(30).IsRequired();

            builder.HasOne(s => s.State).WithMany().HasForeignKey(s => s.StateId);
        }
    }
}
