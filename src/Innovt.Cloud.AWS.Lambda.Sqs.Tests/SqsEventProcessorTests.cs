using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Sqs.Tests
{
    public class Tests
    {
        private TestLambdaContext lambdaContext;

        [SetUp]
        public void Setup()
        {
            lambdaContext = new TestLambdaContext()
            {
                Logger = Substitute.For<ILambdaLogger>()
            };
        }

        [Test]
        public async Task Handle_BatchMessage_Returns_Empty_When_ThereIsNoMessages()
        {
            var processor = new CustomSqsEventProcessor();

            var result = await processor.Process(new Amazon.Lambda.SQSEvents.SQSEvent(), lambdaContext);

            Assert.IsNotNull(result);
            Assert.IsNull(result.BatchItemFailures);            
        }



        [Test]
        public async Task Handle_BatchMessage_ForFifoQueue_Returns_TheRemainingMessages_WhenFindTheFirstFailure()
        {
            var processor = new CustomSqsEventProcessor(true);

            var messages = new List<Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage>();

            var expectedMessageIdFailed = new List<string>() {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
            };

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("michel")),
                MessageId = Guid.NewGuid().ToString(),
            }); ;

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("magal")),
                MessageId = expectedMessageIdFailed[0]
            });

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("ti")),
                MessageId = expectedMessageIdFailed[1]
            });

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("ale")),
                MessageId = expectedMessageIdFailed[2]
            });

            var sQSEvent = new Amazon.Lambda.SQSEvents.SQSEvent()
            {
                Records = messages
            };

            //Will fail when person name is not michel

            var result = await processor.Process(sQSEvent, lambdaContext);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.BatchItemFailures);
            Assert.AreEqual(result.BatchItemFailures.Count, expectedMessageIdFailed.Count);            

            foreach (var item in result.BatchItemFailures)
            {
                if (!expectedMessageIdFailed.Contains(item.ItemIdentifier))
                {
                    Assert.Fail(item.ItemIdentifier);
                }
            }
        }


        [Test]
        public async Task Handle_BatchMessage_Returns_FailedMessages_WhenQueueIsStandard()
        {
            var processor = new CustomSqsEventProcessor(false);

            var messages = new List<Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage>();

            var expectedMessageIdFailed = new List<string>() {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };
            
            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("magal")),
                MessageId = expectedMessageIdFailed[0]
            });

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("michel")),
                MessageId = Guid.NewGuid().ToString(),
            });

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("ale")),
                MessageId = expectedMessageIdFailed[1]
            });

            messages.Add(new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
            {
                Body = System.Text.Json.JsonSerializer.Serialize(new Person("michel")),
                MessageId = Guid.NewGuid().ToString(),
            });

            var sQSEvent = new Amazon.Lambda.SQSEvents.SQSEvent()
            {
                Records = messages
            };

            //Will fail when person name is not michel
            var result = await processor.Process(sQSEvent, lambdaContext);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.BatchItemFailures);
            Assert.AreEqual(result.BatchItemFailures.Count, expectedMessageIdFailed.Count);


            var content = System.Text.Json.JsonSerializer.Serialize(result);

            foreach (var item in result.BatchItemFailures)
            {
                if (!expectedMessageIdFailed.Contains(item.ItemIdentifier))
                {
                    Assert.Fail(item.ItemIdentifier);
                }
            }
        }

        [Test]
        public async Task Handle_BatchMessage_Returns_EmptyBatchResponseWhen_ThereIsNoFailures()
        {
            var processor = new CustomSqsEventProcessor(false);

            var messages = new List<Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage>
            {
                new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
                {
                    Body = System.Text.Json.JsonSerializer.Serialize(new Person("michel")),
                    MessageId = Guid.NewGuid().ToString(),
                },

                new Amazon.Lambda.SQSEvents.SQSEvent.SQSMessage()
                {
                    Body = System.Text.Json.JsonSerializer.Serialize(new Person("michel")),
                    MessageId = Guid.NewGuid().ToString(),
                }
            };

            var sQSEvent = new Amazon.Lambda.SQSEvents.SQSEvent()
            {
                Records = messages
            };

            //Will fail when person name is not michel
            var result = await processor.Process(sQSEvent, lambdaContext);

            Assert.IsNotNull(result);            
            Assert.IsNull(result.BatchItemFailures);            
        }


    }
}