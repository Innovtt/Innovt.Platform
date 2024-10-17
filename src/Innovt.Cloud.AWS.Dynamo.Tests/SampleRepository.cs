using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleRepository(DynamoContext dynamoContext, ILogger logger, IAwsConfiguration configuration)
    : Repository(logger,
        configuration, dynamoContext)
{
    public async Task SaveUser(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        var request = new TransactionWriteRequest();
        //Username only if the expert is new
        request.AddItem(new TransactionWriteItem
        {
            OperationType = TransactionWriteOperationType.Put,
            TableName = "Users",
            ConditionExpression = "attribute_not_exists(PK)",
            Items = new Dictionary<string, object>
            {
                { "PK", $"michelmob{user.Id}" },
                { "SK", "USERNAME" },
                { "Id", user.Id }
            }
        });

        try
        {
            var transactWriteItem = CreateTransactionWriteItem(user);
            transactWriteItem.OperationType = TransactionWriteOperationType.Put;
            transactWriteItem.ConditionExpression = "attribute_not_exists(PK)";
            request.AddItem(transactWriteItem);

            await TransactWriteItemsAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Delete(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        var request = new TransactionWriteRequest();
        //Username only if the expert is new
        request.AddItem(new TransactionWriteItem
        {
            OperationType = TransactionWriteOperationType.Delete,
            TableName = "Users",
            ConditionExpression = "attribute_exists(PK)",
            Keys = new Dictionary<string, object>
            {
                { "PK", $"michelmob{user.Id}" },
                { "SK", "USERNAME" }
            }
        });

        try
        {
            var transactWriteItem = CreateTransactionWriteItem(user, TransactionWriteOperationType.Delete);
            transactWriteItem.ConditionExpression = "attribute_exists(PK)";
            request.AddItem(transactWriteItem);

            await TransactWriteItemsAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}