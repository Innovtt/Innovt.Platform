using System;
using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Implementation of IEntityTypeDataModelMapper for mapping the UserSample entity to its corresponding data model.
/// </summary>
public class UserMap : IEntityTypeDataModelMapper<User>
{
    /// <summary>
    ///     Configures the mapping between the UserSample entity and its corresponding data model using the provided
    ///     EntityTypeBuilder.
    /// </summary>
    /// <param name="builder">The EntityTypeBuilder used to configure the mapping.</param>
    public void Configure([NotNull] EntityTypeBuilder<User> builder)
    {
        builder.AutoMap().HasDefaultKeys().HasTableName("Users", "#");
        builder.HasHashKey().SetDynamicValue(u => "USER#" + u.Id);
        builder.HasRangeKey().HasDefaultValue("PROFILE");
        builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
        builder.HasHashKeyPrefix("USER");
        builder.Ignore(c => c.Company);
        builder.Property(p => p.StatusIds).WithMap(p =>
            p.Status = p.StatusIds.IsNullOrEmpty() ? UserStatus.Active : UserStatus.Inactive);
        //builder.Include(c=>c.StatusIds);
        builder.Property(c => c.Company).WithMap(c => c.Company = new Company
        {
            Name = "Company",
            Id = Guid.NewGuid().ToString()
        });
    }
}