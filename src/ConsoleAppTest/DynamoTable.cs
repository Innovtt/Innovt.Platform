using System;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest;
//[Amazon.DynamoDBv2.DataModel.DynamoDBTable("UserRoles")]
//[Amazon.DynamoDBv2.DataModel.DynamoDBTable("BuyerConfiguratorIntegrator")]
//[Amazon.DynamoDBv2.DataModel.DynamoDBTable("Invoices")]
// [Amazon.DynamoDBv2.DataModel.DynamoDBTable("SupplierConsultancyRegister")]

internal class SupplierConsultancyRegister : ITableMessage
{
    [DynamoDBHashKey("ConsultancyId")] public Guid ConsultancyId { get; set; }

    [DynamoDBRangeKey("SupplierId")] public Guid RangeKey { get; set; }

    public string Status { get; set; }

    public string Id { get; set; }
    //   public string Subject { get; set; }
}