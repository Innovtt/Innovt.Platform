using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Kinesis
{
    public class InvalidEventLimitException:BaseException
    {
        public InvalidEventLimitException():base("Invalid event limit. Kinesis supports up to 500 messages per request.")
        {
            
        }
    }
}