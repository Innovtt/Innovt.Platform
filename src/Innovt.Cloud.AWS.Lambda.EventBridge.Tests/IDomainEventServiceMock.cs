// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge.Tests

using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests;

public interface IDomainEventServiceMock<in T> where T : DomainEvent
{
    IContainer InicializeIoc();
    void ProcessMessage(T domainEvent);
}
