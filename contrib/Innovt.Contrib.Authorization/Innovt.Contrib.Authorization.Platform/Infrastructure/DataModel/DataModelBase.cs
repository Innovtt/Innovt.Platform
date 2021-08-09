using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    [DynamoDBTable("ServicesAuthorization")] 
    internal abstract class DataModelBase: ITableMessage
    { 
        [DynamoDBHashKey("PK")]
        public string Id { get; set; }

        [DynamoDBRangeKey("SK")]
        public string Sk { get; set; }

        public DataModelBase()
        {            
        }

        [DynamoDBProperty]
        public string EntityType { get; set; }      
    }
}