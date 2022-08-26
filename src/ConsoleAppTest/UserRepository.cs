using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleAppTest.DataModels;
using ConsoleAppTest.DataModels.Anticipation;
using ConsoleAppTest.DataModels.AuthorizationTest;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Security;

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

    public async Task<AuthUser> GetUserByExternalId(string externalId,
        CancellationToken cancellationToken = default)
    {
        var request = new QueryRequest
        {
            KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
            Filter = new { pk = $"U#{externalId}", sk = "DID#" }
        };

        var user = await QueryFirstOrDefaultAsync<UserDataModel>(request, cancellationToken).ConfigureAwait(false);

        return UserDataModel.ToUser(user);
    }


    public async Task<List<BidDataModel>> GetBids(CancellationToken cancellationToken = default)
    {
        var queryRequest = new QueryRequest
        {
            KeyConditionExpression = "SK=:sk AND begins_with(PK,:pk)",
            IndexName = "SK-PK-Index",
            Filter = new
            {
                pk = "BID#",
                sk = "BID#Supplier#e9bc5a4e-5291-4aa8-b129-08b2d75ca000#Buyer#e8427bd4-b6d3-4e89-b734-1690cae813a1#DATE#26/08/2022"
            }
        };

        return (await QueryAsync<BidDataModel>(queryRequest, cancellationToken)).ToList();
    }
}