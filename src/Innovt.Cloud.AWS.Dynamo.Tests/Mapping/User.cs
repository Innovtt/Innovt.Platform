using System;
using System.Collections.Generic;
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
    public Uri? Picture { get; set; } //14
}

public class User : BaseUser
{
    private UserStatus status;

    public User()
    {
        //Properties from BaseUser
        Picture2 = "test"; //10 
        CreatedAt = DateTime.Now; //11
        UpdatedAt = DateTime.Now; //12
    }

    public string FirstName { get; set; } //1
    public string LastName { get; set; } //2
    public string Email { get; set; } //3
    public string Context { get; set; } //4
    public bool IsActive { get; set; } //5

    public DateTimeOffset? LastAccess { get; set; } //6
    public int JobPositionId { get; set; } //7
    public new string Id { get; set; } //8
    public List<int> DaysOfWeek { get; set; }

    public UserStatus Status
    {
        get => status;
        set
        {
            status = value;
            if(value!=null)
                StatusId = value.Id;
        }
    }

    public int StatusId { get; set; }
    public Company Company { get; set; }
}