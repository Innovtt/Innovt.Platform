using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Core.CrossCutting.Log;

namespace ConsoleAppTest;

public class UserRepository : Repository
{
    public UserRepository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }





}