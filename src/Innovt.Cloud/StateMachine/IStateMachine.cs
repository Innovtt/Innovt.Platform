// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.StateMachine;

public interface IStateMachine
{
    Task StartExecution(object input, string stateMachineArn, string executionId, CancellationToken cancellationToken);
    Task SendTaskSuccess(string taskToken, object output, CancellationToken cancellationToken);
    Task SendTaskFailure(string taskToken, string reason, string taskError, CancellationToken cancellationToken);
    Task SendTaskHeartbeat(string taskToken, CancellationToken cancellationToken);
}