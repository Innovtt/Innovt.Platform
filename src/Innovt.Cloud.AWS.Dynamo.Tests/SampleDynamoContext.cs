using System;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping.Contacts;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleDynamoContext : DynamoContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        
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
        
        //como pegar o tipo base ? 
        //Quando tem um discriminator, vc deve mapear os 3 tipos.
        modelBuilder.Entity<DynamoContact>().AutoMap().WithTableName("CloudExperts")
            .WithHashKey().SetDynamicValue(c => "CONTACT").Builder
            .WithRangeKey().SetDynamicValue(c => "CONTACT#" + c.Id).Builder
            .HasDiscriminator("Type").HasValue<DynamoPhoneContact>(1)
            .HasValue<DynamoEmailContact>(2);

        modelBuilder.Entity<DynamoPhoneContact>().AutoMap().WithTableName("CloudExperts")
            .WithHashKey().SetDynamicValue(c => "CONTACT").Builder
            .WithRangeKey().SetDynamicValue(c => "CONTACT#" + c.Id);
}
}