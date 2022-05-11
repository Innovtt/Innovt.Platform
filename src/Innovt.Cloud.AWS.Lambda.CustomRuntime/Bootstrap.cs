// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.CustomRuntime
// Solution: Innovt.Platform
// Date: 2021-06-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

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