// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using Innovt.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps;

/// <summary>
///     Configuration for mapping the BaseUser entity to the database using Entity Framework Core.
/// </summary>
public class BaseUserMap : IEntityTypeConfiguration<BaseUser>
{
    /// <summary>
    ///     Configures the mapping for the BaseUser entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when the builder parameter is null.</exception>
    public void Configure(EntityTypeBuilder<BaseUser> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        //'builder.ToTable("User");

        builder.HasKey(u => u.Id);
        builder.Property(b => b.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(b => b.LastName).HasMaxLength(50).IsRequired(false);
        builder.Property(b => b.Email).HasMaxLength(300).IsRequired();
        builder.Property(b => b.Password).HasMaxLength(50).IsRequired(false);
    }
}