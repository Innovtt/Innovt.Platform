// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events.Tests

using System.Text.Json;
using Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.Tests;

[TestFixture]
public class DefineAuthChallengeHandlerTests
{
    [Test]
    public void ParseEvent_Whit_IssueToken_Null_Returns_NUll()
    {
        var jsonEvent = @"{
                            ""request"": {
                                ""userAttributes"": {
                                    ""string"": ""string""                                        
                                },
                                ""session"": [                                                                        
                                ],
                                ""clientMetadata"": {
                                    ""string"": ""string""                                    
                                },
                                ""userNotFound"": true
                            },
                            ""response"": {
                                ""challengeName"": ""string"",                                
                                ""issueTokens"": null,
                                ""failAuthentication"": true
                            }
                        }";

        var message = JsonSerializer.Deserialize<DefineAuthChallengeEvent>(jsonEvent);

        Assert.Multiple(() =>
        {
            Assert.That(message, Is.Not.Null);
            Assert.That(message.Response.IssueTokens, Is.False);
            Assert.That(message.Response.FailAuthentication, Is.EqualTo(true));
        });
    }

    [Test]
    public void ParseEvent_Whit_IssueToken_Returns_Value()
    {
        var jsonEvent = @"{
                            ""request"": {
                                ""userAttributes"": {
                                    ""string"": ""string""                                        
                                },
                                ""session"": [                                                                        
                                ],
                                ""clientMetadata"": {
                                    ""string"": ""string""                                    
                                },
                                ""userNotFound"": true
                            },
                            ""response"": {
                                ""challengeName"": ""string"",                                
                                ""issueTokens"": true,
                                ""failAuthentication"": true
                            }
                        }";

        var message = JsonSerializer.Deserialize<DefineAuthChallengeEvent>(jsonEvent);

        Assert.Multiple(() =>
        {
            Assert.That(message, Is.Not.Null);
            Assert.That(message.Response.IssueTokens, Is.True);
            Assert.That(message.Response.FailAuthentication, Is.EqualTo(true));
        });
    }
}