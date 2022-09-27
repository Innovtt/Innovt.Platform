// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Adresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

public class CityMap : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Name).HasMaxLength(30).IsRequired();

        builder.HasOne(s => s.State).WithMany().HasForeignKey(s => s.StateId);
    }
}