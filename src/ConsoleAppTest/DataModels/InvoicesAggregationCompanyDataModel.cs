using Amazon.DynamoDBv2.DataModel;

namespace ConsoleAppTest.DataModels
{
    [DynamoDBTable("InvoicesAggregation")]
    public class InvoicesAggregationCompanyDataModel
    {
        public string Currency { get; set; } = "BRL";

        public decimal TotalValue { get; set; }

        public int Quantity { get; set; }


        [DynamoDBHashKey]
        public string PK { get; set; }

        [DynamoDBRangeKey]
        public string SK1 { get; set; }


        public string CompanyId { get; set; }

        public string BuildSk1()
        {
            return $"PK#{CompanyId}#SK#UNIQUE";
        }

        public string BuildSk2()
        {
            return $"PK#UNIQUE#SK#{CompanyId}";
        }

    //    public override void EnrichmentAttibuteValueUpdate(Dictionary<string, AttributeValueUpdate> attributeValueUpdate)
    //    {
            
    //        databaseObject.TableName = "InvoicesAggregation";
    //        databaseObject.Keys = new Dictionary<string, AttributeValue>
    //        {
    //            {"PK", new AttributeValue(PK)},
    //            {"SK1", new AttributeValue(BuildSk1())}
    //        };

    //        databaseObject.AttributeValueUpdate = new Dictionary<string, AttributeValueUpdate>
    //        {
    //            {"SK2", new AttributeValueUpdate(new AttributeValue {S = BuildSk2()}, AttributeAction.PUT)},
    //            {"KpiType", new AttributeValueUpdate(new AttributeValue {S = KpiType}, AttributeAction.PUT)},
    //            {
    //                "TotalValue",
    //                new AttributeValueUpdate(new AttributeValue {N = TotalValue.ToString()}, AttributeAction.ADD)
    //            },
    //            {
    //                "Quantity",
    //                new AttributeValueUpdate(new AttributeValue {N = Quantity.ToString()}, AttributeAction.ADD)
    //            },
    //            {"Currency", new AttributeValueUpdate(new AttributeValue {S = Currency}, AttributeAction.PUT)}
    //        };

    //        EnrichmentAttibuteValueUpdate(databaseObject.AttributeValueUpdate);

    //        return databaseObject;
    //    }
   }
}
