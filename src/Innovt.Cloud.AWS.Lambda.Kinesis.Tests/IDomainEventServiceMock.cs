// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests;

public interface IDomainEventServiceMock<T> where T : DomainEvent
{
    IContainer InicializeIoc();
    BatchFailureResponse ProcessMessage(T domainEvent);
}