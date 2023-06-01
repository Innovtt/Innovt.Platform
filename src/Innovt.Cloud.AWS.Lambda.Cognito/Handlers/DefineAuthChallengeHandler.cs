// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class DefineAuthChallengeHandler : EventProcessor<DefineAuthChallengeEvent, DefineAuthChallengeEvent>
{
    protected DefineAuthChallengeHandler(ILogger logger) : base(logger)
    {
    }

    protected DefineAuthChallengeHandler()
    {
    }

}