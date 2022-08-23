using Amazon.DynamoDBv2.DataModel;
using ConsoleAppTest.Domain;
using System.Collections.Generic;

namespace ConsoleAppTest.DataModels;

public class UserDataModel:DataModelBase
{
    public string PK { get; set; }

    public string SK { get; set; }

    public string FirstName { get; set; }

    public DocumentTypeEnum DocumentType { get; set; }

    public List<ContactDataModel> Contacts { get; set; }

    public string[] Names { get; set; }
    public int[] Ages { get; set; }

}