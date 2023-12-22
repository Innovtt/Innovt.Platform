// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

/// <summary>
///     An abstract base class for handling pre-token generation events.
/// </summary>
public abstract class PreTokenGenerationHandler : EventProcessor<PreTokenGenerationEvent, PreTokenGenerationEvent>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PreTokenGenerationHandler" /> class with a logger.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger" /> instance for logging.</param>
    protected PreTokenGenerationHandler(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PreTokenGenerationHandler" /> class.
    /// </summary>
    protected PreTokenGenerationHandler()
    {
    }
}