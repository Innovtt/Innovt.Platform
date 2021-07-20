// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Innovt.Data.EFCore.Maps
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable(nameof(City));

            builder.HasKey(c => c.Id);

            builder.Property(d => d.Name).HasMaxLength(30).IsRequired();

            builder.HasOne(s => s.State).WithMany().HasForeignKey(s => s.StateId);
        }
    }
}