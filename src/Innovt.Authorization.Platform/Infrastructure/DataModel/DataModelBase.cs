using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace Innovt.Authorization.Platform.Infrastructure.DataModel
{
    [DynamoDBTable("Authorization")]
    public abstract class DataModelBase: ITableMessage
    { 
        [DynamoDBHashKey("PK")]
        public string Id { get; set; }

        [DynamoDBRangeKey("SK")]
        public string Sk { get; set; }

        [DynamoDBProperty]
        public string EntityType { get; set; }
    }
}