namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests;

public interface IServiceMock
{
    void InicializeIoc();

    BatchFailureResponse ProcessMessage(string traceId = null);
}