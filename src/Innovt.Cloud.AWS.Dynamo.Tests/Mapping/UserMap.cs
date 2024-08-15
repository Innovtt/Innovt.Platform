using System;
using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Domain.Security;

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
        builder.AutoMap().WithOneTableHashKey().WithOneTableRangeKey().WithTableName("Users");

        builder.HasHashKeyPrefix("User#");
        
        //USER#34189408-d0a1-70fe-b563-969eceabb35b
        
        builder.IgnoreProperty(p => p.EmailToBeIgnored);

        builder.WithProperty(p => p.Name).AsDecimal().HasName("Name2").IsRequired().HasMaxLength(100);
        builder.WithProperty(p => p.JobPosition).HasName("JobPositionId");
        builder.WithProperty(p => p.Email).HasMaxLength(50).IsRequired();
        
        builder.WithProperty(p => p.Role).HasMap(p =>
        {
            p.Role = new Role()
            {
                Id = p.CorrelationId
            };
        });
    }
}