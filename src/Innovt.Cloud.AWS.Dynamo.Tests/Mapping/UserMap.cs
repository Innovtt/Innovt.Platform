using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

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
        builder.AutoMap().WithDefaultKeys().WithTableName("Users", "#");
        builder.WithHashKey().SetDynamicValue(u => "USER#" + u.Id);

        builder.WithRangeKey().WithValue("PROFILE");
        builder.Property(u => u.Email).WithMaxLength(50).IsRequired();

        builder.WithHashKeyPrefix("USER");
    }
}