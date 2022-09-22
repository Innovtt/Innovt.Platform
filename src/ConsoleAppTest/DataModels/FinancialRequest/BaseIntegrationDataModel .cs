﻿using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest.DataModels.FinancialRequest;

[DynamoDBTable("CapitalSourceAnticipation")]
public abstract class BaseIntegrationDataModel : ITableMessage
{
    [DynamoDBHashKey("PK")]
    public string Id { get; set; }



    [DynamoDBRangeKey("SK")]
    public string Sk { get; set; }



    [DynamoDBProperty]
    public string EntityType { get; set; }
}