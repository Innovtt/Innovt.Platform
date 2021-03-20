using System;

namespace Innovt.Cloud.Table
{
    public class TableMessage : ITableMessage
    {
        public TableMessage()
        {
        }

        public TableMessage(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            Id = id;
        }

        public TableMessage(string id, string rangeKey) : this(id)
        {
            if (string.IsNullOrEmpty(rangeKey)) throw new ArgumentNullException(nameof(rangeKey));

            RangeKey = rangeKey;
        }

        public string Id { get; set; }
        public string RangeKey { get; set; }
    }
}