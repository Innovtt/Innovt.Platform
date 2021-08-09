// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore.Authorization
// Solution: Innovt.Platform
// Date: 2021-05-18
// Contact: michel@innovt.com.br or michelmob@gmail.com
using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.Authorization.AspNetCore
{
    public static class SetupExtension
    {
        public static void AddInnovtAuthorization(this IServiceCollection services, string moduleName)
        {            
            _ = new AuthorizationModule(services,moduleName);
            
            services.AddMvc().AddApplicationPart(typeof(RoleController).Assembly);
        }
    }
}