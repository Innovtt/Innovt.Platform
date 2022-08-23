using System;
using Amazon.DynamoDBv2.DataModel;
using ConsoleAppTest.Domain;
using System.Collections.Generic;

namespace ConsoleAppTest.DataModels;

public class ContactDataModel
{
    public string Value { get; set; }
    public bool Deleted { get; set; }
    public DateTime CreatedAt { get; set; }

}