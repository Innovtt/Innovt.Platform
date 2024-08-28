using System;
using Innovt.Cloud.Table;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents a user in the application.
/// </summary>
public class User:ITableMessage
{
    public string PictureUrl { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string Context { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastAccess { get; set; }
    
    public DateTime? CreatedAt { get; set; }

    public int JobPositionId { get; set; }
    public string Id { get; set; }
}