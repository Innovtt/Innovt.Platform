using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Cloud.Table
{
    public class QueryRequest: BaseRequest, ICloneable
    {   
        public string KeyConditionExpression { get; set; }

        
        public object Clone()
        {
            return new QueryRequest()
            {
                AttributesToGet = this.AttributesToGet,
                Filter = this.Filter,
                IndexName = this.IndexName,
                PageSize = this.PageSize,
                PaginationToken = this.PaginationToken
            };
        }
    }
}