// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

/// <summary>
///     Configuration for mapping the DocumentType entity to the database using Entity Framework Core.
/// </summary>
public class DocumentTypeMap : IEntityTypeConfiguration<DocumentType>
{
    /// <summary>
    ///     Configures the mapping for the DocumentType entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(30).IsRequired();
        builder.Property(d => d.Mask).HasMaxLength(70).IsRequired();

        builder.HasOne(d => d.Country).WithMany().HasForeignKey(d => d.CountryId);
    }
}