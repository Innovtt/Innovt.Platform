// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Adresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

/// <summary>
///     Configuration for mapping the AddressType entity to the database using Entity Framework Core.
/// </summary>
public class AddressTypeMap : IEntityTypeConfiguration<AddressType>
{
    /// <summary>
    ///     Configures the mapping for the AddressType entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<AddressType> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(u => u.Id);
        builder.Property(b => b.Name).HasMaxLength(15).IsRequired();
    }
}