// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PostConfirmation;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

/// <summary>
/// An abstract base class for handling post-confirmation events.
/// </summary>
public abstract class PostConfirmationHandler : EventProcessor<PostConfirmationEvent, PostConfirmationEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostConfirmationHandler"/> class with a logger.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> instance for logging.</param>
    protected PostConfirmationHandler(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostConfirmationHandler"/> class.
    /// </summary>
    protected PostConfirmationHandler()
    {
    }
}