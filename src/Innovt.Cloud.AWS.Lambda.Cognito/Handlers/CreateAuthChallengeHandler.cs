// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class CreateAuthChallengeHandler : EventProcessor<CreateAuthChallengeEvent, CreateAuthChallengeEvent>
{
}