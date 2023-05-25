using Amazon;
using Amazon.Scheduler;
using Amazon.Scheduler.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Scheduler;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Serialization;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Bridge
{
    public class SchedulerService : AwsBaseService, ISchedulerService
    {
        private static readonly ActivitySource QueueActivitySource = new("Innovt.Cloud.AWS.Bridge.SchedulerService");

        private ISerializer serializer;
        private ISerializer Serializer => serializer ??= new JsonSerializer();

        public string RoleArn { get; }

        private AmazonSchedulerClient schedulerClient;
        private AmazonSchedulerClient SchedulerClient => schedulerClient ??= CreateService<AmazonSchedulerClient>();

        public SchedulerService(ILogger logger, IAwsConfiguration configuration, string roleArn,
        ISerializer serializer = null) : base(logger, configuration)
        {
            RoleArn = roleArn ?? throw new System.ArgumentNullException(nameof(roleArn));
            this.serializer = serializer;
        }

        public SchedulerService(ILogger logger, IAwsConfiguration configuration, string region, string roleArn,
            ISerializer serializer = null) : base(logger, configuration, region)
        {
            RoleArn = roleArn ?? throw new System.ArgumentNullException(nameof(roleArn));
            this.serializer = serializer;
        }

        private async Task<string> BaseScheduleQueueMessageAsync<TK>(TK message, string queueName, string scheduleExpression, string scheduleName, string scheduleGroupName = null,
        CancellationToken cancellationToken = default)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));
            if (scheduleName == null) throw new ArgumentNullException(nameof(scheduleName));

            using var activity = QueueActivitySource.StartActivity("ScheduleQueueAsync");
            activity?.SetTag("schedulerService.queueName", queueName);

            var target = new Target()
            {
                Arn = await GetQueueArnAsync(queueName).ConfigureAwait(false),
                Input = Serializer.SerializeObject(message),
                RoleArn = RoleArn,
                RetryPolicy = new RetryPolicy()
                {
                    MaximumRetryAttempts = 3
                }
            };
            var flexibleTimeWindow = new FlexibleTimeWindow()
            {
                Mode = FlexibleTimeWindowMode.OFF
            };

            CreateScheduleResponse response;

            try
            {
                response = await base.CreateDefaultRetryAsyncPolicy()
                    .ExecuteAsync(async () =>
                        await SchedulerClient.CreateScheduleAsync(new CreateScheduleRequest()
                        {
                            Name = scheduleName,
                            State = ScheduleState.ENABLED,
                            ScheduleExpression = scheduleExpression,
                            GroupName = scheduleGroupName,
                            Target = target,
                            FlexibleTimeWindow = flexibleTimeWindow,
                        }, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (Amazon.Scheduler.Model.ConflictException ex)
            {
                throw new ScheduleConflictException(ex.Message);
            }

            activity?.SetTag("schedulerService.status_code", response.HttpStatusCode);

            if (response == null || response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error schedule message to AWS Bridge.");

            return response.ResponseMetadata.RequestId;
        }

        public async Task<string> ScheduleQueueMessageAsync<TK>(TK message, string queueName, DateTime dateTime, string scheduleName, string scheduleGroupName = null,
        CancellationToken cancellationToken = default)
        {
            return await BaseScheduleQueueMessageAsync(message, queueName, $"at({dateTime:yyyy-MM-ddTHH:mm:ss})", scheduleName, scheduleGroupName: scheduleGroupName, cancellationToken: cancellationToken);
        }

        public async Task<string> ScheduleQueueMessageAsync<TK>(TK message, string queueName, string cronExpression, string scheduleName, string scheduleGroupName = null,
        CancellationToken cancellationToken = default)
        {
            return await BaseScheduleQueueMessageAsync(message, queueName, cronExpression, scheduleName, scheduleGroupName: scheduleGroupName, cancellationToken: cancellationToken);
        }

        public async Task DeleteSchedulerAsync(string scheduleName, string scheduleGroupName = null, CancellationToken cancellationToken = default)
        {
            if (scheduleName == null) throw new ArgumentNullException(nameof(scheduleName));
            using var activity = QueueActivitySource.StartActivity("ScheduleDeleteAsync");
            activity?.SetTag("schedulerService.Name", scheduleName);
            if (!string.IsNullOrWhiteSpace(scheduleGroupName))
                activity?.SetTag("schedulerService.GroupName", scheduleGroupName);

            DeleteScheduleResponse response;
            try
            {
                response = await base.CreateDefaultRetryAsyncPolicy()
                            .ExecuteAsync(async () =>
                                await SchedulerClient.DeleteScheduleAsync(new DeleteScheduleRequest()
                                {
                                    Name = scheduleName,
                                    GroupName = scheduleGroupName,
                                }, cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (Amazon.Scheduler.Model.ResourceNotFoundException)
            {
                throw new ScheduleNotFoundException($"Scheduler {scheduleName} not found.");
            }

            activity?.SetTag("schedulerService.status_code", response.HttpStatusCode);

            if (response == null || response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error on delete scheduler at AWS Bridge.");
        }

        private async Task<string> GetQueueArnAsync(string queueName)
        {
            string queueArn = null;

            using var activity = QueueActivitySource.StartActivity();
            activity?.SetTag("schedulerService.queue_name", queueName);

            if (Configuration?.AccountNumber != null)
            {
                queueArn = $"arn:aws:sqs:{GetServiceRegionEndPoint()?.SystemName ?? RegionEndpoint.USEast1.SystemName}:{Configuration.AccountNumber}:{queueName}";
            }
            else
            {
                IAmazonSecurityTokenService stsClient = CreateService<AmazonSecurityTokenServiceClient>();
                var accountNumber = (await stsClient.GetCallerIdentityAsync(new GetCallerIdentityRequest()).ConfigureAwait(false)).Account;

                queueArn = $"arn:aws:sqs:{GetServiceRegionEndPoint()?.SystemName ?? RegionEndpoint.USEast1.SystemName}:{accountNumber}:{queueName}";
            }
            activity?.SetTag("schedulerService.queue_arn", queueArn);

            return queueArn;
        }

        protected override void DisposeServices()
        {
            schedulerClient?.Dispose();
        }
    }
}