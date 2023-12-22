// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Adresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

/// <summary>
/// Configuration for mapping the Address entity to the database using Entity Framework Core.
/// </summary>
public class AddressMap : IEntityTypeConfiguration<Address>
{
    private readonly bool ignoreCity;
    private readonly bool ignoreCoordinate;
    private readonly bool ignoreType;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressMap"/> class.
    /// </summary>
    /// <param name="ignoreCoordinate">Flag indicating whether to ignore coordinate mapping. Defaults to false.</param>
    /// <param name="ignoreCity">Flag indicating whether to ignore city mapping. Defaults to false.</param>
    /// <param name="ignoreType">Flag indicating whether to ignore type mapping. Defaults to false.</param>
    public AddressMap(bool ignoreCoordinate = false, bool ignoreCity = false, bool ignoreType = false)
    {
        this.ignoreCoordinate = ignoreCoordinate;
        this.ignoreCity = ignoreCity;
        this.ignoreType = ignoreType;
    }

    /// <summary>
    /// Configures the mapping for the Address entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));


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

        if (ignoreCoordinate) builder.Ignore(b => b.Coordinate);

        //todo:map coordinates
    }
}