using System.ComponentModel.DataAnnotations;
using Innovt.Domain.Core.Model;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class Skill : ValueObject<int>
{
    public Skill()
    {
    }

    public Skill(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [Required] [StringLength(50)] public string Name { get; set; }

    [StringLength(100)] public string Description { get; set; }

}