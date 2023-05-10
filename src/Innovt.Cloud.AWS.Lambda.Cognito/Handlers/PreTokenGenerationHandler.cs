// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class PreTokenGenerationHandler : EventProcessor<PreTokenGenerationEvent, PreTokenGenerationEvent>
{
    protected PreTokenGenerationHandler(ILogger logger) : base(logger)
    {
    }

    protected PreTokenGenerationHandler()
    {
    }
}