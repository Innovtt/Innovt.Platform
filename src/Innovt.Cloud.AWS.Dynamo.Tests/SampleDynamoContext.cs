using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using System;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleDynamoContext: DynamoContext
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.AddConfiguration(new UserMap());
    }
}