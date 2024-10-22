using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class UserStatus:ConstantClass
{
    private static List<UserStatus> StatusList = [];
    public static readonly UserStatus Active = new UserStatus(1, "Active");
    public static readonly UserStatus Inactive = new UserStatus(2, "Inactive");
    protected UserStatus(int id, string value) : base(value)
    {
        this.Id = id;
        StatusList.Add(this);
    }

    public UserStatus():base("teste") 
    {
        
    }
    public int Id { get; set; }
    
    public UserStatus GetById(int id)
    {
        return StatusList.SingleOrDefault(s => s.Id == id);
    }
}