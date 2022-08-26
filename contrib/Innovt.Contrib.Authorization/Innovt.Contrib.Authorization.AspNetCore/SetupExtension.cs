// Company: Antecipa
// Project: Innovt.Contrib.Authorization.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.Authorization.AspNetCore
{
    public static class SetupExtension
    {
        public static void AddInnovtAuthorization(this IServiceCollection services, string moduleName)
        {
            _ = new AuthorizationModule(services);

            services.AddMvc().AddApplicationPart(typeof(RolesController).Assembly);
        }
    }
}