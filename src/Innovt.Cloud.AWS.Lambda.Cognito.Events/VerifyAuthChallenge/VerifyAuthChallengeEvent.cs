// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

public class VerifyAuthChallengeEvent : CognitoTriggerEvent<VerifyAuthChallengeRequest,
    VerifyAuthChallengeResponse>
{
}