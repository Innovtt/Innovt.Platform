using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Mapping;
/// <summary>
/// Implementation of IEntityTypeDataModelMapper for mapping the UserSample entity to its corresponding data model.
/// </summary>
public class UserMap : IEntityTypeDataModelMapper<UserSample>
{
    /// <summary>
    /// Configures the mapping between the UserSample entity and its corresponding data model using the provided EntityTypeBuilder.
    /// </summary>
    /// <param name="builder">The EntityTypeBuilder used to configure the mapping.</param>
    public void Configure(EntityTypeBuilder<UserSample> builder)
    {
        builder.AutoMap().WithOneTableHashKey().WithOneTableRangeKey().WithTableName(nameof(UserSample));
        
        builder.IgnoreProperty(p=>p.Email);

        builder.WithProperty(p => p.Name).AsDecimal().HasName("NameID");
        
        builder.WithTableName(nameof(UserSample));
    }
}