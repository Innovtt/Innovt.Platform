using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests
{
    public interface IServiceMock
    {

        void InicializeIoc();


        void ProcessMessage(string traceId=null);
    }
}
