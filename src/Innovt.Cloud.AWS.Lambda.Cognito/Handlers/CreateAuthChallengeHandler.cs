// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class CreateAuthChallengeHandler : EventProcessor<CreateAuthChallengeEvent, CreateAuthChallengeEvent>
{
    protected CreateAuthChallengeHandler(ILogger logger):base(logger)
    {
    }

    protected CreateAuthChallengeHandler()
    {

    }
}