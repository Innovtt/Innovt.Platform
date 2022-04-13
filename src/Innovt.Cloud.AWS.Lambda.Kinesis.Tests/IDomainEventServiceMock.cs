using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests
{
    public interface IDomainEventServiceMock<T> where T : DomainEvent
    {
        void InicializeIoc();
        BatchFailureResponse ProcessMessage(T domainEvent);
    }
}
