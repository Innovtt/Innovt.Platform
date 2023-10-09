using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Mapping;

public interface IEntityTypeDataModelMapper<T> where T : class
{
    public void Configure(EntityTypeBuilder<T> builder);
}