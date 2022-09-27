// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;

namespace Innovt.Cloud.Table;

public class QueryRequest : BaseRequest, ICloneable
{
    public string KeyConditionExpression { get; set; }

    public bool ScanIndexForward { get; set; }

    public object Clone()
    {
        return new QueryRequest
        {
            AttributesToGet = AttributesToGet,
            Filter = Filter,
            KeyConditionExpression = KeyConditionExpression,
            FilterExpression = FilterExpression,
            IndexName = IndexName,
            ScanIndexForward = ScanIndexForward,
            PageSize = PageSize,
            Page = Page
        };
    }
}