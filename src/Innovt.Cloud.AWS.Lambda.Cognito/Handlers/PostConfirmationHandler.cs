// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PostConfirmation;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class PostConfirmationHandler : EventProcessor<PostConfirmationEvent, PostConfirmationEvent>
{
    protected PostConfirmationHandler(ILogger logger) : base(logger)
    {
    }

    protected PostConfirmationHandler()
    {
    }
 
}