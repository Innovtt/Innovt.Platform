// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Innovt.Data.EFCore.Maps
{
    public class DocumentMap : IEntityTypeConfiguration<Document>
    {
        private readonly bool ignoreDocumentType;

        public DocumentMap(bool ignoreDocumentType = false)
        {
            this.ignoreDocumentType = ignoreDocumentType;
        }

        public void Configure(EntityTypeBuilder<Document> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable(nameof(Document));

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
}