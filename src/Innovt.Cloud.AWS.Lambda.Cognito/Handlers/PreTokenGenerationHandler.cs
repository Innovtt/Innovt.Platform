// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito

using Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Handlers;

public abstract class PreTokenGenerationHandler : EventProcessor<PreTokenGenerationEvent, PreTokenGenerationEvent>
{
}