using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc
{
    public class IOCModule
    {
        private readonly IServiceCollection _services;

        public IOCModule(IServiceCollection services)
        {
            _services = services;
        }

        public IOCModule()
        {
            _services = new ServiceCollection();
        }

        public IServiceCollection GetServices()
        {
            return _services;
        }
    }
}