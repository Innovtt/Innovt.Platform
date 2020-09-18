using System;

namespace Innovt.Cloud.Table
{
    public class TableMessage:ITableMessage
    {
        public TableMessage()
        {
        }

        public TableMessage(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            this.Id = id;
        }

        public TableMessage(string id,string rangeKey):this(id)
        {
            if (string.IsNullOrEmpty(rangeKey)) throw new ArgumentNullException(nameof(rangeKey));

            this.RangeKey = rangeKey;
        }

        public string Id { get; set; }
        public string RangeKey { get; set; }
    }
}