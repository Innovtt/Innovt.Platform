// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PostConfirmation;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class PostConfirmationHandler : EventProcessor<PostConfirmationEvent, PostConfirmationEvent>
{
}