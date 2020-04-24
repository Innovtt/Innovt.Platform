using System;

namespace Innovt.Cloud.Table
{
    public class TableMessage:ITableMessage
    {
        public TableMessage()
        {
        }
        
        public TableMessage(string key, string partitionKey)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(partitionKey)) throw new ArgumentNullException(nameof(partitionKey));
  
   
            this.Id = key;
            this.PartitionKey = partitionKey;
        }

        public string Id { get; set; }

        public string PartitionKey { get; set; }

    }
}