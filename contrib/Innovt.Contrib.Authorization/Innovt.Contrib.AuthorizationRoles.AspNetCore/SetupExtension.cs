// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using Innovt.AspNetCore.Handlers;
using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;

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


        services.AddScoped<IAuthorizationRepository>(provider =>
            provider.GetRequiredService<Authorization.Platform.Domain.IAuthorizationRepository>()
        );

        services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
    }
}