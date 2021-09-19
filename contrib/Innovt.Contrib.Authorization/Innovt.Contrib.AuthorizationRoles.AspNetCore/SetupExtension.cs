// Company: Antecipa
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-17

using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore
{
    public static class SetupExtension
    {
        public static void AddInnovtRolesAuthorization(this IServiceCollection services, string moduleName)
        {
            _ = new AuthorizationModule(services, moduleName);

            services.AddMvc().AddApplicationPart(typeof(RolesController).Assembly);
        }
    }
}