using Innovt.Cloud.Table;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents a user in the application.
/// </summary>
public class Company : ITableMessage
{
    public string Name { get; set; }

    public User User { get; set; }

    public string Id { get; set; }
}