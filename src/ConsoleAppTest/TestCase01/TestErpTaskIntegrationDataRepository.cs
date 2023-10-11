using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace ConsoleAppTest.TestCase01;

public class TestErpTaskIntegrationDataRepository : Innovt.Cloud.AWS.Dynamo.Repository
{
    public TestErpTaskIntegrationDataRepository(ILogger logger, IAwsConfiguration awsConfiguration) : base(logger,
        awsConfiguration)
    {
    }

    public async Task<TestTaskIntegration> GetTaskIntegrationDocument(CancellationToken cancellationToken = default)
    {
        var modelFilter = new
        {
            pk = $"BUYER#b80ea7d6-63d3-45e1-b51c-ce73299fc443",
            sk = "TASK_INTEGRATION#5d43ef08-fdfe-4064-ac81-f24855304ffb"
        };

        var request = new Innovt.Cloud.Table.QueryRequest()
        {
            KeyConditionExpression = $"PK = :pk AND SK = :sk",
            Filter = modelFilter
        };

        var results = await QueryAsync<TestErpTaskIntegrationData>(request, cancellationToken);

        return null;
    }

    public async Task<TestTaskIntegration> GetTaskIntegrationAnticipate(CancellationToken cancellationToken = default)
    {
        var modelFilter = new
        {
            pk = $"BUYER#221b0a76-059f-4d3f-8cc4-8450f0a141b6",
            sk = "TASK_INTEGRATION#acc41cdb-3ea6-47cb-bf75-74a8d9f59eb5"
        };

        var request = new Innovt.Cloud.Table.QueryRequest()
        {
            KeyConditionExpression = $"PK = :pk AND SK = :sk",
            Filter = modelFilter
        };

        var results = await QueryAsync<TestErpTaskIntegrationData>(request, cancellationToken);

        return null;
    }
}