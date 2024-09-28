using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class BaseUserOne:Entity<Guid>
{
}

/// <summary>
///     Represents a user in the application.
/// </summary>
/// 
public class BaseUser:BaseUserOne
{
    public string Picture2 { get; set; }
    public string Picture { get; set; }
}

public class User:BaseUser
{
    public string FirstName { get; set; } //1
    public string LastName { get; set; } //2
    public string Email { get; set; } //3
    public string Context { get; set; } //4
    public bool IsActive { get; set; } //5

    public DateTime? LastAccess { get; set; } //6
    public new DateTimeOffset? CreatedAt { get; set; }  //7
    public int JobPositionId { get; set; } //8
    public new string Id { get; set; } //9

    public User()
    {
        //Properties from BaseUser
        this.Picture = "test"; //10
        this.Picture2 = "test"; //11 
    }
}