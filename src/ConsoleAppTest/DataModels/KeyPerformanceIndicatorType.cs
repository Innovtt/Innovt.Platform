// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using Amazon.DynamoDBv2.DataModel;

namespace ConsoleAppTest.DataModels;

[DynamoDBTable("KeyPerformanceIndicatorType")]
public class KeyPerformanceIndicatorType
{
    public string Context { get; set; }

    public string Type { get; set; }

    public bool IsActive { get; set; }
}