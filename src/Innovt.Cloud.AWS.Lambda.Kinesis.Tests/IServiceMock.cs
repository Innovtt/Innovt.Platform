// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests;

public interface IServiceMock
{
    IContainer InitializeIoc();

    BatchFailureResponse ProcessMessage(string? traceId = null);
}