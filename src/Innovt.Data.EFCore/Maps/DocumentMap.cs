// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;
/// <summary>
/// Configuration for mapping the Document entity to the database using Entity Framework Core.
/// </summary>
public class DocumentMap : IEntityTypeConfiguration<Document>
{
    private readonly bool ignoreDocumentType;
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentMap"/> class.
    /// </summary>
    /// <param name="ignoreDocumentType">Flag indicating whether to ignore document type mapping. Defaults to false.</param>
    public DocumentMap(bool ignoreDocumentType = false)
    {
        this.ignoreDocumentType = ignoreDocumentType;
    }
    /// <summary>
    /// Configures the mapping for the Document entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(d => d.Id);
        builder.Property(e => e.Number).HasMaxLength(20).IsRequired();

        if (ignoreDocumentType)
        {
            builder.Ignore(d => d.DocumentType);
            builder.Ignore(d => d.DocumentTypeId);
        }
        else
        {
            builder.HasOne(d => d.DocumentType).WithMany().HasForeignKey(d => d.DocumentTypeId);
        }
    }
}