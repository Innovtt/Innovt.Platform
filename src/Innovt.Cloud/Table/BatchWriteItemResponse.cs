using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table
{
    public class BatchWriteItemResponse
    {
        public Dictionary<string, List<BatchWriteItem>> UnprocessedItems { get; private set; }

        public BatchWriteItemResponse()
        {
            UnprocessedItems = new Dictionary<string, List<BatchWriteItem>> ();
        }

        public bool Success => UnprocessedItems is null || UnprocessedItems.Count == 0;
    }
}

