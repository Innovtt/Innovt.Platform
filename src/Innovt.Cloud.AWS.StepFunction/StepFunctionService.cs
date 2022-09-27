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

public class StepFunctionService : AwsBaseService, IStateMachine
{
    private static readonly ActivitySource StepFunctionActivitySource = new("Innovt.Cloud.AWS.StepFunction");

    private AmazonStepFunctionsClient awStepFunctionClient;

    public StepFunctionService(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    public StepFunctionService(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }

    private AmazonStepFunctionsClient StepFunctionClient
    {
        get { return awStepFunctionClient ??= CreateService<AmazonStepFunctionsClient>(); }
    }

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

    protected override void DisposeServices()
    {
        awStepFunctionClient?.Dispose();
    }
}