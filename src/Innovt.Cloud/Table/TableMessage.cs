// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

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

        public string RangeKey { get; set; }

        public string Id { get; set; }
    }
}