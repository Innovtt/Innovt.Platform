
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace Antecipa.BI.Platform.Infrastructure.Database.DataModel
{
    [DynamoDBTable("BiDashboards")]
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