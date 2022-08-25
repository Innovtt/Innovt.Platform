using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleAppTest.DataModels;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;

namespace ConsoleAppTest;

public class UserRepository : Repository
{
    public UserRepository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    public async Task<AuthProviderDataModel> GetAuthProvider()
    {
        try
        {
            var request = new Innovt.Cloud.Table.QueryRequest()
            {
                KeyConditionExpression = $"PK = :pk AND SK = :sk",
                Filter = new { pk = AuthProviderDataModel.BuildPk(), sk = "bradesco.com.br" }
            };

            var authProvider = await base.QueryFirstOrDefaultAsync<AuthProviderDataModel>(request, CancellationToken.None);


            return authProvider;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CapitalSourceDataModel> GetCapitalSources(CancellationToken cancellationToken = default)
    {
        var request = new QueryRequest()
        {
            KeyConditionExpression = $"PK = :pk AND SK = :sk",
            Filter = new { pk = $"CS#93f8149f-e1f4-4721-8c7e-9dbc761b0f32", sk = "Profile" },
        };
        var result = await QueryFirstOrDefaultAsync<CapitalSourceDataModel>(request, cancellationToken);

        return result;
    }

}