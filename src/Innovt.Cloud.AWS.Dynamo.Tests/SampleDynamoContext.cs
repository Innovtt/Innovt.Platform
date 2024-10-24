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

        modelBuilder.Entity<Skill>().AutoMap().HasTableName("CloudExperts")
            .HasHashKey().SetDynamicValue(c => "SKILL").Builder
            .HasRangeKey().SetDynamicValue(c => "SKILL#" + c.Id);
            
        modelBuilder.Entity<CloudExpertSkill>().AutoMap().HasTableName("CloudExperts")
            .HasHashKey().SetDynamicValue(c => "CE#" + c.OwnerId).Builder
            .HasRangeKey().SetDynamicValue(c => "CE#SKILL#" + c.Id);

        modelBuilder.Entity<Availability>().AutoMap().HasTableName("CloudExperts")
            .HasHashKey().SetDynamicValue(c => "CE#" + c.OwnerId).Builder
            .HasRangeKey().SetDynamicValue(c => "CE#AVAILABILITY").Builder
            .Ignore(p => p.DayOfWeek);
        
        //como pegar o tipo base ? 
        //Quando tem um discriminator, vc deve mapear os 3 tipos.
        modelBuilder.Entity<DynamoContact>().AutoMap().HasTableName("CloudExperts")
            .HasHashKey().SetDynamicValue(c => "CONTACT").Builder
            .HasRangeKey().SetDynamicValue(c => "CONTACT#" + c.Id).Builder
            .HasDiscriminator("Type").HasValue<DynamoPhoneContact>(1)
            .HasValue<DynamoEmailContact>(2);

        //Se vc nao mapeia o sistema vai considerar apenas o que esta mapeado na super classe. 
        
        modelBuilder.Entity<DynamoPhoneContact>().AutoMap().HasTableName("CloudExperts")
            .HasHashKey().SetDynamicValue(c => "CONTACT").Builder
            .HasRangeKey().SetDynamicValue(c => "CONTACT#" + c.Id);

        modelBuilder.Entity<DynamoEmailContact>().AutoMap().HasTableName("CloudExperts");
    }
}