using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Cloud.Table
{
    public class QueryRequest: BaseRequest, ICloneable
    {
        public string KeyConditionExpression { get; set; }
        
        public bool ScanIndexForward { get; set; }
 
        public object Clone()
        {
            return new QueryRequest()
            {
                AttributesToGet = this.AttributesToGet,
                Filter = this.Filter,
                KeyConditionExpression = this.KeyConditionExpression,
                FilterExpression = this.FilterExpression,
                IndexName = this.IndexName,
                ScanIndexForward = this.ScanIndexForward,
                PageSize = this.PageSize,
                Page = this.Page
            };
        }
    }
}