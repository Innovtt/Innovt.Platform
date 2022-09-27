// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests;

public interface IDataServiceMock<T> where T : class
{
    void InicializeIoc();


    void ProcessMessage(IDataStream<T> domainEvent);
}