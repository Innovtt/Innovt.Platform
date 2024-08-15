using System;
using Innovt.Cloud.Table;
using Innovt.Domain.Security;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents a user in the application.
/// </summary>
public class User:ITableMessage
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public int JobPosition { get; set; }
    public string EmailToBeIgnored { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }

    public string Id { get; set; }
    public Role Role { get; set; }
    
    public int RoleId { get; set; }
    
    public Guid CorrelationId { get; set; }
}