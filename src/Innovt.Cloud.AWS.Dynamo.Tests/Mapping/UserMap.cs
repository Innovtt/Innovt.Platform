using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class UserMap : IEntityTypeDataModelMapper<UserSample>
{
    public void Configure(EntityTypeBuilder<UserSample> builder)
    {
        builder.AutoMap().WithOneTableHashKey().WithOneTableRangeKey().WithTableName(nameof(UserSample));

        builder.IgnoreProperty(p => p.Email);

        builder.WithProperty(p => p.Name).AsDecimal().HasName("NameID");

        builder.WithTableName(nameof(UserSample));
    }
}