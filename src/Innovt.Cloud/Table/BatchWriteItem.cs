using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table
{
    public class BatchWriteItem
    {
        public Dictionary<string, object> PutRequest { get; set; }

        public Dictionary<string, object> DeleteRequest { get; set; }
    }
}

