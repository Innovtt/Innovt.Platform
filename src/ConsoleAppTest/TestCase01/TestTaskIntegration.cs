using System;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Domain.Core.Model;

namespace ConsoleAppTest.TestCase01
{
    [DynamoDBTable("TaskIntegration")]
    public class TestTaskIntegration : Entity
    {
        public new Guid Id { get; set; }
        public string TaskType { get; set; }
        public int? StatusId { get; set; }
        public bool Automatic { get; set; }

        [DynamoDBIgnore]
        public TestTaskStatus Status
        {
            get
            {
                if (StatusId.HasValue)
                    return TestTaskStatus.GetByPk(StatusId.Value);
                else
                    return null;
            }
        }

        [DynamoDBProperty(typeof(TestDateTimeUtcConverter))]
        public DateTime CreatedOn { get; set; }

        [DynamoDBProperty(typeof(TestDateTimeUtcConverter))]
        public DateTime? ClosedOn { get; set; }

        [DynamoDBProperty(typeof(TestParametersConverter))]
        public ITestParameters Parameters { get; set; }
        public Guid? CurrentTaskRequestId { get; private set; }
        public string ErrorDescription { get; set; }

        [DynamoDBIgnore]
        public new DateTimeOffset CreatedAt { get; set; }
    }
}