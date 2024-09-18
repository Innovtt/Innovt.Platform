using System;
using Innovt.Cloud.AWS.Dynamo.Converters;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleDynamoContext : DynamoContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        //tenho que dizer que a propriedade do pk sera o campo x +
        //modelBuilder.Entity<User>().AutoMap()
          // .WithOneTableHashKey();
          
        modelBuilder.AddConfiguration(new UserMap());
        modelBuilder.AddConfiguration(new CompanyMap());
        
        modelBuilder.AddPropertyConverter(typeof(DateTimeOffset), new DateTimeOffsetConverter());
        modelBuilder.AddPropertyConverter(typeof(DateTimeOffset?), new DateTimeOffsetConverter());
    }
}