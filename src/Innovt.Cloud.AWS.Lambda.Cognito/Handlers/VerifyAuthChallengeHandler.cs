// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

/// <summary>
///     An abstract base class for handling verification of authentication challenges.
/// </summary>
public abstract class VerifyAuthChallengeHandler : EventProcessor<VerifyAuthChallengeEvent, VerifyAuthChallengeEvent>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VerifyAuthChallengeHandler" /> class with a logger.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger" /> instance for logging.</param>
    protected VerifyAuthChallengeHandler(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="VerifyAuthChallengeHandler" /> class.
    /// </summary>
    protected VerifyAuthChallengeHandler()
    {
    }
}