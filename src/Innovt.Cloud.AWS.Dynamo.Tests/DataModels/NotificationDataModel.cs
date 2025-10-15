// Innovt Company
// Author: Michel Borges
// Project: Cloud2Gether.Notifications.Platform

using System;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace Innovt.Cloud.AWS.Dynamo.Tests.DataModels;

[DynamoDBTable("NotificationRequests")]
public class NotificationDataModel
{
    [DynamoDBRangeKey("SK")] public string Sk { get; set; }

    [DynamoDBHashKey("PK")] public string Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string TemplateId { get; set; }

    public string To { get; set; }

    public string PayLoad { get; set; }

    public string Status { get; set; }
    public string LatestErrorMessage { get; set; }
    public DateTime? LatestErrorAt { get; set; }
    public int ApproximateReceiveCount { get; set; }
}