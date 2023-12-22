// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

/// <summary>
/// An abstract base class for handling the definition of authentication challenges.
/// </summary>
public abstract class DefineAuthChallengeHandler : EventProcessor<DefineAuthChallengeEvent, DefineAuthChallengeEvent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefineAuthChallengeHandler"/> class with a logger.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> instance for logging.</param>
    protected DefineAuthChallengeHandler(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefineAuthChallengeHandler"/> class.
    /// </summary>
    protected DefineAuthChallengeHandler()
    {
    }
}