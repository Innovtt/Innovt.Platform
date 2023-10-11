// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Adresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

/// <summary>
/// Configuration for mapping the Country entity to the database using Entity Framework Core.
/// </summary>
public class CountryMap : IEntityTypeConfiguration<Country>
{
    private readonly bool ignoreIsoCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryMap"/> class.
    /// </summary>
    /// <param name="ignoreIsoCode">Flag indicating whether to ignore ISO code mapping. Defaults to false.</param>
    public CountryMap(bool ignoreIsoCode = false)
    {
        this.ignoreIsoCode = ignoreIsoCode;
    }

    /// <summary>
    /// Configures the mapping for the Country entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Name).HasMaxLength(30).IsRequired();

        builder.Property(d => d.ISOCode).HasMaxLength(3).IsRequired();
        builder.Property(d => d.Nationality).HasMaxLength(20).IsRequired();

        if (ignoreIsoCode)
            builder.Ignore(d => d.Code);
        else
            builder.Property(d => d.Code).HasMaxLength(3).IsRequired();
    }
}