// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PostConfirmation;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PostAuthentication;

/// <summary>
///     Represents an event for post-confirmation actions.
/// </summary>
public class PostAuthenticationEvent : TriggerEvent<PostAuthenticationRequest, PostAuthenticationResponse>
{
}