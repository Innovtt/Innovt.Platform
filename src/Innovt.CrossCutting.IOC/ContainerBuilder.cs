// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.CrossCutting.IOC
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC
{
    public class ContainerBuilder
    {
        private readonly IContainer container;

        public ContainerBuilder(IServiceCollection services)
        {
            container = new Container(services);
        }

        public IServiceProvider GetServiceProvider()
        {
            return new ServiceProvider(container);
        }
    }
}