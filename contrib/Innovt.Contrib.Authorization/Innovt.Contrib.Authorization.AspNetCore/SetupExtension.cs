// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore.Authorization
// Solution: Innovt.Platform
// Date: 2021-05-18
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Reflection;
using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.Authorization.AspNetCore
{
    public static class SetupExtension
    {
        public static void AddInnovtAuthorization(this IServiceCollection services, string moduleName,Assembly hostAssembly)
        {
            //var applicationPart =  new ApplicationPa
            _ = new AuthorizationModule(services);

            services.AddMvc().AddApplicationPart(typeof(RoleController).Assembly);
        }
    }
}