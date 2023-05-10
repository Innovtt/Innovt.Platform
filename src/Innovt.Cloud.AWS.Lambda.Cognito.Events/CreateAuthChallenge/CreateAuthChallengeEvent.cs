// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

public class CreateAuthChallengeEvent : CognitoTriggerEvent<CreateAuthChallengeRequest,
    CreateAuthChallengeResponse>
{
}