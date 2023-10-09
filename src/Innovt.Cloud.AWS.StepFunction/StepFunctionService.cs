// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.StepFunction

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.StateMachine;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.StepFunction;
/// <summary>
/// Represents a service for interacting with AWS Step Functions.
/// </summary>
public class StepFunctionService : AwsBaseService, IStateMachine
{
    private static readonly ActivitySource StepFunctionActivitySource = new("Innovt.Cloud.AWS.StepFunction");

    private AmazonStepFunctionsClient awStepFunctionClient;
    /// <summary>
    /// Initializes a new instance of the StepFunctionService class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The AWS configuration instance.</param>
    public StepFunctionService(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }
    /// <summary>
    /// Initializes a new instance of the StepFunctionService class with a specific region.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The AWS configuration instance.</param>
    /// <param name="region">The AWS region.</param>
    public StepFunctionService(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }

    private AmazonStepFunctionsClient StepFunctionClient
    {
        get { return awStepFunctionClient ??= CreateService<AmazonStepFunctionsClient>(); }
    }
    /// <summary>
    /// Starts the execution of a state machine.
    /// </summary>
    /// <param name="input">The input for the execution.</param>
    /// <param name="stateMachineArn">The ARN of the state machine.</param>
    /// <param name="executionId">The execution ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StartExecution(object input, string stateMachineArn, string executionId,
        CancellationToken cancellationToken)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (stateMachineArn == null) throw new ArgumentNullException(nameof(stateMachineArn));
        if (executionId == null) throw new ArgumentNullException(nameof(executionId));

        using var activity = StepFunctionActivitySource.StartActivity();
        activity?.SetTag("StateMachine.Arn", stateMachineArn);
        activity?.SetTag("StateMachine.ExecutionName", executionId);

        var policy = base.CreateDefaultRetryAsyncPolicy();

        await policy.ExecuteAsync(async () =>
                await StepFunctionClient.StartExecutionAsync(new StartExecutionRequest()
                {
                    Input = System.Text.Json.JsonSerializer.Serialize(input),
                    StateMachineArn = stateMachineArn,
                    Name = executionId
                }, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Sends a success response for a task in the state machine.
    /// </summary>
    /// <param name="taskToken">The token for the task.</param>
    /// <param name="output">The output of the task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendTaskSuccess(string taskToken, object output, CancellationToken cancellationToken)
    {
        if (taskToken == null) throw new ArgumentNullException(nameof(taskToken));
        if (output == null) throw new ArgumentNullException(nameof(output));

        using var activity = StepFunctionActivitySource.StartActivity();
        activity?.SetTag("StateMachine.SendTaskSuccess", taskToken);

        var policy = base.CreateDefaultRetryAsyncPolicy();

        await policy.ExecuteAsync(async () =>
                await StepFunctionClient.SendTaskSuccessAsync(new SendTaskSuccessRequest()
                {
                    TaskToken = taskToken,
                    Output = System.Text.Json.JsonSerializer.Serialize(output)
                }, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Sends a failure response for a task in the state machine.
    /// </summary>
    /// <param name="taskToken">The token for the task.</param>
    /// <param name="reason">The reason for the failure.</param>
    /// <param name="taskError">The error associated with the failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendTaskFailure(string taskToken, string reason, string taskError,
        CancellationToken cancellationToken)
    {
        if (taskToken == null) throw new ArgumentNullException(nameof(taskToken));
        if (reason == null) throw new ArgumentNullException(nameof(reason));

        using var activity = StepFunctionActivitySource.StartActivity();
        activity?.SetTag("StateMachine.SendTaskFailure", taskToken);

        var policy = base.CreateDefaultRetryAsyncPolicy();

        await policy.ExecuteAsync(async () =>
                await StepFunctionClient.SendTaskFailureAsync(new SendTaskFailureRequest()
                {
                    TaskToken = taskToken,
                    Cause = reason,
                    Error = taskError
                }, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Sends a heartbeat for a task in the state machine.
    /// </summary>
    /// <param name="taskToken">The token for the task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendTaskHeartbeat(string taskToken, CancellationToken cancellationToken)
    {
        if (taskToken == null) throw new ArgumentNullException(nameof(taskToken));

        using var activity = StepFunctionActivitySource.StartActivity();
        activity?.SetTag("StateMachine.SendTaskFailure", taskToken);

        var policy = base.CreateDefaultRetryAsyncPolicy();

        await policy.ExecuteAsync(async () =>
                await StepFunctionClient.SendTaskHeartbeatAsync(new SendTaskHeartbeatRequest()
                {
                    TaskToken = taskToken
                }, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Disposes of the Amazon Step Functions client.
    /// </summary>
    protected override void DisposeServices()
    {
        awStepFunctionClient?.Dispose();
    }
}