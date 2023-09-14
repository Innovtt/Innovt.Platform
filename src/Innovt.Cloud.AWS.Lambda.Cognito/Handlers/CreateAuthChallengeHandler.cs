// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

/// <summary>
/// An abstract base class for handling authentication challenge creation events.
/// </summary>
public abstract class CreateAuthChallengeHandler : EventProcessor<CreateAuthChallengeEvent, CreateAuthChallengeEvent>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAuthChallengeHandler"/> class with a logger.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> instance for logging.</param>
    protected CreateAuthChallengeHandler(ILogger logger):base(logger)
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAuthChallengeHandler"/> class.
    /// </summary>
    protected CreateAuthChallengeHandler()
    {

    }
}