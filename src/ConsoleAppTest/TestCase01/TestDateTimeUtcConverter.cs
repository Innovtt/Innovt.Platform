using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ConsoleAppTest.TestCase01;

public class TestDateTimeUtcConverter : IPropertyConverter
{
    public DynamoDBEntry ToEntry(object value)
    {
        return (DateTime)value;
    }

    public object FromEntry(DynamoDBEntry entry)
    {
        var dateTime = entry.AsDateTime();
        return dateTime.ToUniversalTime();
    }
}