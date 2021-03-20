using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    public class BaseRepository:Repository
    {
        public BaseRepository(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
           
        }

        public BaseRepository(ILogger logger, IAWSConfiguration configuration, string region) : base(logger, configuration, region)
        {
            
        }
        
        
    }
}