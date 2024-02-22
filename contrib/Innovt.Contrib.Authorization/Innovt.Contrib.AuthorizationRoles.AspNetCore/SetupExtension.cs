// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using Innovt.AspNetCore.Handlers;
using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;

/// <summary>
///     Extension methods for configuring authorization services.
/// </summary>
public static class SetupExtension
{
    /// <summary>
    ///     Configures authorization services for Innovt Roles Admin.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to configure the services.</param>
    public static void AddInnovtRolesAdminAuthorization(this IServiceCollection services)
    {
        _ = new AuthorizationModule(services);

        services.AddMvc().AddApplicationPart(typeof(UsersController).Assembly);
    }

    /// <summary>
    ///     Configures authorization services for Innovt Roles.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to configure the services.</param>
    public static void AddInnovtRolesAuthorization(this IServiceCollection services)
    {
        _ = new AuthorizationModule(services);


        services.AddScoped<IAuthorizationRepository>(provider =>
            provider.GetRequiredService<Authorization.Platform.Domain.IAuthorizationRepository>()
        );

        services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
    }
}