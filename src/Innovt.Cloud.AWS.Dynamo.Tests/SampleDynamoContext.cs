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

        //modelBuilder.IgnoreNonNativeTypes = true;
        modelBuilder.AddConfiguration(new UserMap());

        modelBuilder.Entity<Skill>().AutoMap().WithTableName("CloudExperts")
            .WithHashKey().SetDynamicValue(c => "SKILL").Builder
            .WithRangeKey().SetDynamicValue(c => "SKILL#" + c.Id);

        modelBuilder.Entity<CloudExpertSkill>().AutoMap().WithTableName("CloudExperts")
            .WithHashKey().SetDynamicValue(c => "CE#" + c.OwnerId).Builder
            .WithRangeKey().SetDynamicValue(c => "CE#SKILL#" + c.Id);

        modelBuilder.Entity<Availability>().AutoMap().WithTableName("CloudExperts")
            .WithHashKey().SetDynamicValue(c => "CE#" + c.OwnerId).Builder
            .WithRangeKey().SetDynamicValue(c => "CE#AVAILABILITY").Builder
            .Ignore(p => p.DayOfWeek);

        //modelBuilder.AddPropertyConverter(typeof(DateTimeOffset), new DateTimeOffsetConverter());
        //modelBuilder.AddPropertyConverter(typeof(DateTimeOffset?), new DateTimeOffsetConverter());
    }
}