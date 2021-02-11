using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Kinesis
{
    internal class InvalidEventLimitException:BaseException
    {
        internal InvalidEventLimitException():base("Invalid event limit. Kinesis supports up to 500 messages per request.")
        {
            
        }
    }
}