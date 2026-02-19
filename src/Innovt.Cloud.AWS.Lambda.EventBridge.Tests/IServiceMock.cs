// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge.Tests

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests;

public interface IServiceMock
{
    IContainer InitializeIoc();

    void ProcessMessage(string? traceId = null);
}
