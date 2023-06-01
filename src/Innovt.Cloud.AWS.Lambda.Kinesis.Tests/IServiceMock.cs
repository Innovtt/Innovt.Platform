// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests;

public interface IServiceMock
{
    void InitializeIoc();

    BatchFailureResponse ProcessMessage(string? traceId = null);
}