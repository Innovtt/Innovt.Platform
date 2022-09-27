// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ConsoleAppTest.DataModels;
using ConsoleAppTest.DataModels.CapitalSource.DataModels;
using ConsoleAppTest.DataModels.FinancialRequest;
using ConsoleAppTest.Domain;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Cloud.Table;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using BatchWriteItemRequest = Innovt.Cloud.Table.BatchWriteItemRequest;
using QueryRequest = Innovt.Cloud.Table.QueryRequest;

namespace ConsoleAppTest;

public class InvoiceRepository : Repository
{
    public InvoiceRepository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    public async Task BatchInsert(IList<InvoicesAggregationCompanyDataModel> dataModels)
    {
        try
        {
            var transactionRequest = new BatchWriteItemRequest();

            foreach (var dataModel in dataModels)
            {
                transactionRequest.AddItem("MichelSample", new BatchWriteItem()
                {
                    PutRequest = new Dictionary<string, object>()
                    {
                        { "PK", dataModel.PK },
                        { "SK", dataModel.SK1 },
                        { "TotalValue", dataModel.TotalValue },
                        {
                            "Quantity", dataModel.Quantity
                        }
                    }
                });


                //transactionRequest.TransactItems.Add(new TransactionWriteItem()
                //{
                //    TableName = "InvoicesAggregation",
                //    Keys = new Dictionary<string, object>
                //    {
                //        { "PK", dataModel.PK },
                //        { "SK1", dataModel.SK1 }
                //    },
                //    OperationType = TransactionWriteOperationType.Check,
                //    UpdateExpression = "SET TotalValue = TotalValue + :tot, Quantity = Quantity + :qty ",
                //    ExpressionAttributeValues = new Dictionary<string, object>
                //    {
                //        {
                //            ":tot",dataModel.TotalValue
                //        },
                //        {
                //            ":qty",dataModel.TotalValue
                //        }
                //    }
                //});
            }

            await BatchWriteItem(transactionRequest, CancellationToken.None);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task SaveAll(IList<InvoicesAggregationCompanyDataModel> dataModels)
    {
        try
        {
            //Cria as entradas antes,
            //atualiza as entradas
            var transactionRequest = new TransactionWriteRequest();

            foreach (var dataModel in dataModels)
            {
                //transactionRequest.TransactItems.Add(new TransactionWriteItem()
                //{
                //    TableName = "InvoicesAggregation",
                //    Items = new Dictionary<string, object>
                //    {
                //        { "PK", dataModel.PK },
                //        { "SK1", dataModel.SK1 },
                //        {
                //            "TotalValue",dataModel.TotalValue
                //        },
                //        {
                //            "Quantity",dataModel.Quantity
                //        }
                //    },
                //    OperationType = TransactionWriteOperationType.Put,
                //    ConditionExpression = "attribute_not_exists(PK)",

                //Todos os registros podem ser da mesma particao. 
                // 1- 100 supplier ->  100 
                //A -> 2 
                //B -> 2


                //});

                //transactionRequest.TransactItems.Add(new TransactionWriteItem()
                //{
                //    TableName = "InvoicesAggregation",
                //    Keys = new Dictionary<string, object>
                //{
                //    { "PK", dataModel.PK },
                //    { "SK1", dataModel.SK1 }
                //},
                //    OperationType = TransactionWriteOperationType.Update,
                //    UpdateExpression = "SET TotalValue = TotalValue + :tot, Quantity = Quantity + :qty ",
                //    ExpressionAttributeValues = new Dictionary<string, object>
                //    {
                //        {
                //            ":tot",dataModel.TotalValue
                //        },
                //        {
                //            ":qty",dataModel.Quantity
                //        }
                //    }
                //});
                //transactionRequest.TransactItems.Add(new TransactionWriteItem()
                //{
                //    TableName = "InvoicesAggregation",
                //    Keys = new Dictionary<string, object>
                //    {
                //        { "PK", dataModel.PK },
                //        { "SK1", dataModel.SK1 }
                //    },
                //    OperationType = TransactionWriteOperationType.Check,
                //    UpdateExpression = "SET TotalValue = TotalValue + :tot, Quantity = Quantity + :qty ",
                //    ExpressionAttributeValues = new Dictionary<string, object>
                //    {
                //        {
                //            ":tot",dataModel.TotalValue
                //        },
                //        {
                //            ":qty",dataModel.TotalValue
                //        }
                //    }
                //});
            }

            await TransactWriteItemsAsync(transactionRequest, CancellationToken.None);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        //await UpdateAsync(databaseObject.TableName, databaseObject.Keys, databaseObject.AttributeValueUpdate, cancellationToken);
    }

    //public async Task<List<UserDataModel>> GetBatchUsers()
    //{
    //    try
    //    {
    //        //Cria as entradas antes,
    //        //atualiza as entradas
    //        var getRequest = new BatchGetItemRequest();

    //        getRequest.AddItem("Users", new BatchGetItem()
    //        {
    //            ConsistentRead = true,
    //            Keys = new List<Dictionary<string, object>>
    //            {
    //                new Dictionary<string, object>(){
    //                    {"PK", "U#b9909dfa-bd94-4370-80be-43d183545185" },
    //                    {"SK", "PROFILE" }
    //                },
    //                 new Dictionary<string, object>(){
    //                    {"PK", "U#9828d40c-9122-4cbf-89e7-f53ca0de9b18" },
    //                    {"SK", "PROFILE" }
    //                },
    //                 new Dictionary<string, object>(){
    //                    {"PK", "U#9c3db849-a690-4c60-afd2-0af09fc755f2" },
    //                    {"SK", "PROFILE" }
    //                }
    //            }
    //        }); 

    //        return await BatchGetItem<UserDataModel>(getRequest, CancellationToken.None);
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //        throw;
    //    }
    //    //await UpdateAsync(databaseObject.TableName, databaseObject.Keys, databaseObject.AttributeValueUpdate, cancellationToken);
    //}

    public async Task<int> IncrementOrderSequence(CancellationToken cancellationToken)
    {
        var keys = new Dictionary<string, AttributeValue>
        {
            { "PK", new AttributeValue("ORDER") },
            { "SK", new AttributeValue("SEQUENCE") }
        };

        var updateValues = new Dictionary<string, AttributeValueUpdate>
        {
            {
                "Sequence",
                new AttributeValueUpdate(new AttributeValue { N = "1" }, AttributeAction.ADD)
            }
        };

        var res = await UpdateAsync<SequenceResponse>(new UpdateItemRequest
        {
            AttributeUpdates = updateValues,
            Key = keys,
            TableName = AnticipationRequestDataModel.TableName,
            ReturnValues = ReturnValue.UPDATED_NEW
        }, CancellationToken.None).ConfigureAwait(false);


        return res?.Sequence ?? 0;
    }

    public async Task<ContractDataModel> GetTaxRequests()
    {
        var contractsRequest = new QueryRequest()
        {
            KeyConditionExpression = $"PK = :pk AND begins_with(EntityTypeCreatedAt,:sk) ",
            Filter = new
            {
                pk = $"CS#52b02587-bea0-41d1-bd0d-6b2f40181be7", sk = $"ET#Contract",
                buyerid = "e8427bd4-b6d3-4e89-b734-1690cae813a1", deleted = false
            },
            FilterExpression = "contains(BuyersIds, :buyerid) AND Deleted = :deleted ",
            IndexName = "PK-EntityTypeCreatedAt-Index",
            ScanIndexForward = false
        };

        var contractResult =
            await QueryFirstOrDefaultAsync<ContractDataModel>(contractsRequest, CancellationToken.None);

        return contractResult;
    }

    public async Task<KeyPerformanceIndicatorType> GetKeyIndicators()
    {
        var contractsRequest = new QueryRequest()
        {
            KeyConditionExpression = $"Context = :ct",
            Filter = new { ct = $"Michel" }
        };

        var contractResult =
            await QueryFirstOrDefaultAsync<KeyPerformanceIndicatorType>(contractsRequest, CancellationToken.None);

        return contractResult;
    }

    public async Task<FinancialRequestIntegrationDataModel> GetRequestIntegration()
    {
        var result = await QueryAsync<FinancialRequestIntegrationDataModel>(new QueryRequest()
        {
            Filter = new
                { financialrequestid = "de784c99-1f80-48ee-919b-f42c9fe053d6", entitytype = "FinancialRequest" },
            IndexName = "FinancialRequestId-EntityType-Index",
            KeyConditionExpression = "EntityType = :entitytype AND FinancialRequestId = :financialrequestid"
        }, CancellationToken.None);


        return result.SingleOrDefault();
    }


    public async Task<PagedCollection<KpiProgressDataModel>> GetKpiProgress()
    {
        var queryRequest = new QueryRequest
        {
            KeyConditionExpression = "PK = :pk",
            Filter = new { pk = "E#88d4c8c7fb435a16b0d777b42cc85bab" }
        };

        return await QueryPaginatedByAsync<KpiProgressDataModel>(queryRequest, CancellationToken.None);
    }
}