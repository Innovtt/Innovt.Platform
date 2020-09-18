using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Core.CrossCutting.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest
{
    public class UserRepository:Repository
    {
        public UserRepository(ILogger logger,IAWSConfiguration configuration):base(logger, configuration)
        {
            
        }
    }
}
