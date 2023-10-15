using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleDynamoContext: DynamoContext
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddConfiguration(new UserMap());
    }
}