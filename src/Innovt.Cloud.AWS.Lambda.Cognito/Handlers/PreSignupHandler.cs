// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PreSignup;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

/// <summary>
/// An abstract base class for handling pre-signup generation events.
/// </summary>
public abstract class PreSignupHandler : EventProcessor<PreSignupEvent, PreSignupEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PreSignupHandler"/> class with a logger.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> instance for logging.</param>
    protected PreSignupHandler(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PreSignupHandler"/> class.
    /// </summary>
    protected PreSignupHandler()
    {
    }
}