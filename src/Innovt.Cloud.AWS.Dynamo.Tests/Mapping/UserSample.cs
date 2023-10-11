using System;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class UserSample
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}