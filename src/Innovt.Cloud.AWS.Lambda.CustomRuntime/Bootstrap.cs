// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.CustomRuntime

using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace Innovt.Cloud.AWS.Lambda.CustomRuntime;

public static class Bootstrap
{
    public static async Task RunAsync<T>(Func<T, ILambdaContext, Task> func, CancellationToken cancellationToken)
    {
        using var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer());
        using var bootstrap = new LambdaBootstrap(handlerWrapper);
        await bootstrap.RunAsync(cancellationToken).ConfigureAwait(false);
    }
}