using System.Collections.Generic;
using System.Linq.Expressions;

namespace Innovt.Cloud.AWS.Dynamo
{
    public class ScanQuery
    {
        public string IndexName { get; set; }

        public int PageSize { get; set; }

        public string PaginationToken { get; set; }


        public List<string> AttributesToGet { get; set; }


        public Expression Filter { get; set; }
        
        //query filter
    }
}