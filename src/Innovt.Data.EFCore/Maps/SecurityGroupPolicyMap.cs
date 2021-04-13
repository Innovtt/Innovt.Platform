// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innovt.Data.EFCore.Maps
{
    public class SecurityGroupPolicyMap : IEntityTypeConfiguration<SecurityGroupPolicy>
    {
        public void Configure(EntityTypeBuilder<SecurityGroupPolicy> builder)
        {
            builder.ToTable(nameof(SecurityGroupPolicy));

            builder.HasKey(u => u.Id);
            builder.HasOne(p => p.SecurityGroup);
            builder.HasOne(p => p.Policy);
        }
    }
}