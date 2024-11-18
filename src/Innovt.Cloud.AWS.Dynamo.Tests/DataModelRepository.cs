using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class DataModelRepository(ILogger logger, IAwsConfiguration configuration)
    : Repository(logger,
        configuration)
{
}