// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Adresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;
/// <summary>
/// Configuration for mapping the City entity to the database using Entity Framework Core.
/// </summary>
public class CityMap : IEntityTypeConfiguration<City>
{
    /// <summary>
    /// Configures the mapping for the City entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<City> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Name).HasMaxLength(30).IsRequired();

        builder.HasOne(s => s.State).WithMany().HasForeignKey(s => s.StateId);
    }
}