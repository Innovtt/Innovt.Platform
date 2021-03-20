using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc
{
    public class IOCModule
    {
        private readonly IServiceCollection services;

        public IOCModule()
        {
            this.services = new ServiceCollection();
        }

        public IServiceCollection GetServices()
        {
            return this.services;
        }
    }
}