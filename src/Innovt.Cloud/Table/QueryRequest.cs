// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;

namespace Innovt.Cloud.Table
{
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
}