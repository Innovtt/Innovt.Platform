// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Innovt.Data.EFCore.Maps
{
    public class BaseUserMap : IEntityTypeConfiguration<BaseUser>
    {
        public void Configure(EntityTypeBuilder<BaseUser> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(b => b.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(b => b.LastName).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.Email).HasMaxLength(300).IsRequired();
            builder.Property(b => b.Password).HasMaxLength(50).IsRequired(false);
        }
    }
}