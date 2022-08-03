using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table
{
    public class BatchGetItemRequest
    {
        public Dictionary<string, BatchGetItemRequestItem> Items { get; private set; }

        public BatchGetItemRequest()
        {
            Items = new Dictionary<string, BatchGetItemRequestItem>();
        }

        public BatchGetItemRequest(string tableName, BatchGetItemRequestItem batchRequestItem):this()
        {
            this.AddItem(tableName, batchRequestItem);
        }
     
     
        public void AddItem(string tableName, BatchGetItemRequestItem batchRequestItem)
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

