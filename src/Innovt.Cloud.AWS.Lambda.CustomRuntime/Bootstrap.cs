// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.CustomRuntime

using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace Innovt.Cloud.AWS.Lambda.CustomRuntime;

/// <summary>
/// The Bootstrap class provides a method to run asynchronous Lambda functions.
/// </summary>
public static class Bootstrap
{
    // <summary>
    /// Runs an asynchronous Lambda function using the provided handler function and cancellation token.
    /// </summary>
    /// <typeparam name="T">The type of input the Lambda function expects.</typeparam>
    /// <param name="func">The Lambda function handler to execute.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task RunAsync<T>(Func<T, ILambdaContext, Task> func, CancellationToken cancellationToken)
    {
        using var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer());
        using var bootstrap = new LambdaBootstrap(handlerWrapper);
        await bootstrap.RunAsync(cancellationToken).ConfigureAwait(false);
    }
}