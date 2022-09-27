// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

public class DocumentTypeMap : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(30).IsRequired();
        builder.Property(d => d.Mask).HasMaxLength(70).IsRequired();

        builder.HasOne(d => d.Country).WithMany().HasForeignKey(d => d.CountryId);
    }
}