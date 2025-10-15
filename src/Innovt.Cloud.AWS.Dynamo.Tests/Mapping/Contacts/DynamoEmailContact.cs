using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping.Contacts;

public class DynamoEmailContact : DynamoContact
{
    public string Value { get; set; }

    public List<int> Days { get; set; } = [];

    public DynamoEmailContact()
    {
        Type = 2;
    }
}