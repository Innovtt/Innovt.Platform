// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-18
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.IOC
{
    public class AuthorizationModule:IOCModule
    {
        public AuthorizationModule(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IAuthorizationAppService, AuthorizationAppService>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        }
    }
}