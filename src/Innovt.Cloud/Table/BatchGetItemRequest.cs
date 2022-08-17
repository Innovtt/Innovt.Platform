using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table
{
    public class BatchGetItemRequest
    {
        public Dictionary<string, BatchGetItem> Items { get; private set; }

        public BatchGetItemRequest()
        {
            Items = new Dictionary<string, BatchGetItem>();
        }

        public BatchGetItemRequest(string tableName, BatchGetItem batchRequestItem):this()
        {
            this.AddItem(tableName, batchRequestItem);
        }
     
     
        public void AddItem(string tableName, BatchGetItem batchRequestItem)
        {
            if (batchRequestItem is null)
            {
                throw new ArgumentNullException(nameof(batchRequestItem));
            }

            if(Items.TryGetValue(tableName, out var tableItem))
            {
                tableItem.Keys.AddRange(batchRequestItem.Keys);
            }
            else
            {
                Items.Add(tableName, batchRequestItem);
            }

        }

    }
}

