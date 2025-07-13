using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC.Tests.Mock;

public class IocModule2:IocModule
{
    public IocModule2() : base()
    {
        GetServices().AddTransient<IServiceB, ServiceB>();
    }
    
}