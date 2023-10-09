// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.StateMachine;
/// <summary>
/// Represents a state machine interface.
/// </summary>
public interface IStateMachine
{
    /// <summary>
    /// Starts the execution of the state machine.
    /// </summary>
    /// <param name="input">The input for the state machine.</param>
    /// <param name="stateMachineArn">The Amazon Resource Name (ARN) of the state machine.</param>
    /// <param name="executionId">The execution ID for this execution.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StartExecution(object input, string stateMachineArn, string executionId, CancellationToken cancellationToken);
    /// <summary>
    /// Sends a success signal for a task to the state machine.
    /// </summary>
    /// <param name="taskToken">The token representing the task.</param>
    /// <param name="output">The output of the task.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendTaskSuccess(string taskToken, object output, CancellationToken cancellationToken);
    /// <summary>
    /// Sends a failure signal for a task to the state machine.
    /// </summary>
    /// <param name="taskToken">The token representing the task.</param>
    /// <param name="reason">The reason for task failure.</param>
    /// <param name="taskError">The error associated with the task failure.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendTaskFailure(string taskToken, string reason, string taskError, CancellationToken cancellationToken);
    /// <summary>
    /// Sends a heartbeat signal for a task to the state machine.
    /// </summary>
    /// <param name="taskToken">The token representing the task.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendTaskHeartbeat(string taskToken, CancellationToken cancellationToken);
}