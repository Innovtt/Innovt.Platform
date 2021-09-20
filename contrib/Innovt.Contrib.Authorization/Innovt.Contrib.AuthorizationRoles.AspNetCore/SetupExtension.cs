// Company: Antecipa
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-17

using Innovt.AspNetCore.Handlers;
using Innovt.Contrib.Authorization.Platform.Infrastructure;
using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore
{
    public static class SetupExtension
    {
        public static void AddInnovtRolesAdminAuthorization(this IServiceCollection services)
        {
            _ = new AuthorizationModule(services);

            services.AddMvc().AddApplicationPart(typeof(UsersController).Assembly);
        }

        public static void AddInnovtRolesAuthorization(this IServiceCollection services)
        {
            _ = new AuthorizationModule(services);

            var provider = services.BuildServiceProvider();

            services.AddScoped<IAuthorizationRepository>(_ => provider.GetService<AuthorizationRepository>());

            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
        }
    }
}