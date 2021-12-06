namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests
{
    public interface IServiceMock
    {

        void InicializeIoc();

        IList<BatchFailureResponse> ProcessMessage(string traceId=null);
    }
}
