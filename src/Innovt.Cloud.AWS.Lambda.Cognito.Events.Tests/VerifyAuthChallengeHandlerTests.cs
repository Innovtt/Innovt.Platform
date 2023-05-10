// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events.Tests

using System.Text.Json;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.Tests;

[TestFixture]
public class VerifyAuthChallengeHandlerTests
{
    [Test]
    public void Parse_With_ChallengeAnswer_AsString_Returns_TheValue()
    {
        var jsonEvent = @"{
                    ""request"": {
                        ""userAttributes"": {
                            ""string"": ""string""                            
                        },
                        ""privateChallengeParameters"": {
                            ""string"": ""string""                            
                        },
                        ""challengeAnswer"": ""123456"",
                        ""clientMetadata"": {
                            ""string"": ""string""                            
                        },
                        ""userNotFound"": true
                    },
                    ""response"": {
                        ""answerCorrect"": false
                    }
                }";

        var message = JsonSerializer.Deserialize<VerifyAuthChallengeEvent>(jsonEvent);


        Assert.Multiple(() =>
        {
            Assert.That(message, Is.Not.Null);
            Assert.That(message.Request, Is.Not.Null);
            Assert.That(message.Request.ChallengeAnswer, Is.EqualTo("123456"));
            Assert.That(message.Response.AnswerCorrect, Is.False);
        });
    }
}