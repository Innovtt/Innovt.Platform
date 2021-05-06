// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc
{
    public class IOCModule
    {
        private readonly IServiceCollection services;

        public IOCModule()
        {
            services = new ServiceCollection();
        }

        public IServiceCollection GetServices()
        {
            return services;
        }
    }
}