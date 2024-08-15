using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using System;
using Innovt.Cloud.AWS.Dynamo.Converters;
using Innovt.Cloud.AWS.Dynamo.Mapping;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleDynamoContext: DynamoContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        
        modelBuilder.AddConfiguration(new UserMap());
        modelBuilder.AddConfiguration(new CompanyMap());

        modelBuilder.AddPropertyConverter(typeof(DateTimeOffset), new DateTimeOffsetConverter());
    }
}