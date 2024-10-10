using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class CloudExpertSkill : ValueObject<Guid>
{

    public CloudExpertSkill()
    {
        Id = Guid.NewGuid();
    }
    
    public Guid OwnerId { get; set; }
    
    
    public Skill Skill { get; set; }
    
    public int SkillId { get; set; }
    
    public int ProficiencyId { get; set; }
    public int YearsOfExperience { get; set; }
}