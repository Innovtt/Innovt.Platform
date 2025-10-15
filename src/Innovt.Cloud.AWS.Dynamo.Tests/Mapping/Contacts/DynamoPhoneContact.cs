namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping.Contacts;

public class DynamoPhoneContact : DynamoContact
{
    public string CountryCode { get; set; }

    public DynamoPhoneContact()
    {
        Type = 1;
    }
}