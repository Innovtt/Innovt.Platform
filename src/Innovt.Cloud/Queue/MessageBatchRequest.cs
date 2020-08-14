using System;
using System.Collections.Generic;
using System.Text;

namespace Innovt.Cloud.Queue
{
    public class MessageBatchRequest
    {
        public string Id { get; set; }

        public object Message { get; set; }
    }
}
