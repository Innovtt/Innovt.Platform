using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using ConsoleAppTest.Domain;

namespace ConsoleAppTest.DataModels;

[DynamoDBTable("Users")]
public class UserDataModel 
{
    [DynamoDBProperty("Contacts")] 
    public new List<Contact> Contacts { get; set; }
}