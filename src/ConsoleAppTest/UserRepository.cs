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
        public UserRepository(ILogger logger):base(
            logger,new DefaultAWSConfiguration("AKIA6CVS5EYKTMRWNMXL", "9IfUL2rKc9QPTrPKivHu1bDE49cH8LnSBRg2Pb22", "us-east-1"))
        {

        }
    }
}
