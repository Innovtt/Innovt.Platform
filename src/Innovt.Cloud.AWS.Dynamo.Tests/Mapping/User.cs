using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class BaseUserOne : Entity<Guid>
{
}

/// <summary>
///     Represents a user in the application.
/// </summary>
public class BaseUser : BaseUserOne
{
    public string Picture2 { get; set; } // 13
    public string Picture { get; set; } //14
}

public class User : BaseUser
{
    public User()
    {
        //Properties from BaseUser
        Picture = "test"; //9
        Picture2 = "test"; //10 
        CreatedAt = DateTime.Now; //11
        UpdatedAt = DateTime.Now; //12
    }

    public string FirstName { get; set; } //1
    public string LastName { get; set; } //2
    public string Email { get; set; } //3
    public string Context { get; set; } //4
    public bool IsActive { get; set; } //5

    public DateTime? LastAccess { get; set; } //6
    public int JobPositionId { get; set; } //7
    public new string Id { get; set; } //8

    public Company Company { get; set; }
}