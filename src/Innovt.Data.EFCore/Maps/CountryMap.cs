// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class CountryMap : IEntityTypeConfiguration<Country>
    {
        private readonly bool ignoreIsoCode;

        public CountryMap(bool ignoreIsoCode = false)
        {
            this.ignoreIsoCode = ignoreIsoCode;
        }


        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable(nameof(Country));

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
}