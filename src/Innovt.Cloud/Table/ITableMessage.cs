using System;

namespace Innovt.Cloud.Table
{
    public interface ITableMessage
    {
        string Id { get; set; }

        string PartitionKey { get; set; }
    }
}