// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito
using Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class VerifyAuthChallengeHandler : EventProcessor<VerifyAuthChallengeEvent, VerifyAuthChallengeEvent>
{
    protected VerifyAuthChallengeHandler(ILogger logger) : base(logger)
    {
    }

    protected VerifyAuthChallengeHandler()
    {
    }
}