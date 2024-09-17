using Innovt.Cloud.Table;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents a user in the application.
/// </summary>
public class Address:ITableMessage
{
    public string Street { get; set; }
    
    public string Id { get; set; }
}