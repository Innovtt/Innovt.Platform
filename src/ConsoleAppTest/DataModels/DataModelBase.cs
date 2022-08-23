using Amazon.DynamoDBv2.DataModel;
using ConsoleAppTest.Domain;
using System.Collections.Generic;

namespace ConsoleAppTest.DataModels;

public abstract class DataModelBase
{
    public string PK { get; set; }

    public string SK { get; set; }

}