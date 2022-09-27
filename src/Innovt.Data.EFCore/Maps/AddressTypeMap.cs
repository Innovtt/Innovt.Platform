// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Adresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

public class AddressTypeMap : IEntityTypeConfiguration<AddressType>
{
    public void Configure(EntityTypeBuilder<AddressType> builder)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        builder.HasKey(u => u.Id);
        builder.Property(b => b.Name).HasMaxLength(15).IsRequired();
    }
}