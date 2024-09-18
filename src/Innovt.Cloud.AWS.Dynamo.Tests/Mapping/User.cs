using System;
using Innovt.Cloud.Table;
using Innovt.Domain.Core.Model;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents a user in the application.
/// </summary>
public class BaseUser:Entity
{
    public new string Id { get; set; }
}

public class User :BaseUser
{
    public string Picture { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Context { get; set; }
    public bool IsActive { get; set; }

    public DateTime? LastAccess { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public int JobPositionId { get; set; }
   // public new string Id { get; set; }
}