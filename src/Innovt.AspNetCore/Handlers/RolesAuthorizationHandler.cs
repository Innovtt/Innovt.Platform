// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Security.Claims;
using Innovt.AspNetCore.Extensions;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Innovt.AspNetCore.Handlers;

/// <summary>
/// Authorization handler for role-based authorization.
/// </summary>
public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
{
    private const string contextSeparator = "::";
    private readonly ILogger logger;
    private readonly IAuthorizationRepository securityRepository;

    /// <summary>
    /// Constructs a new instance of <see cref="RolesAuthorizationHandler"/>.
    /// </summary>
    /// <param name="securityRepository">The security repository for retrieving user information.</param>
    /// <param name="logger">The logger for logging messages.</param>
    public RolesAuthorizationHandler(IAuthorizationRepository securityRepository, ILogger logger)
    {
        this.securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the user ID from the authorization context.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <returns>The user ID or an empty string if not found.</returns>
    private static string GetUserId(AuthorizationHandlerContext context)
    {
        var userId = context.User?.GetClaim(ClaimTypes.NameIdentifier);

        return userId ?? string.Empty;
    }

    /// <summary>
    /// Gets the application context from the authorization context.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <returns>The application context.</returns>
    private static string GetApplicationContext(AuthorizationHandlerContext context)
    {
        var scope = string.Empty;

        if (context.Resource is not HttpContext httpContext) return scope;

        if (httpContext.Request.Headers.TryGetValue(Constants.HeaderApplicationScope, out var appScope))
            scope = appScope.ToString();

        if (!httpContext.Request.Headers.TryGetValue(Constants.HeaderApplicationContext, out var headerContext))
            return scope;

        if (!httpContext.Request.Headers.TryGetValue(headerContext, out var applicationContext))
            return scope;

        if (applicationContext.IsNullOrEmpty())
            return scope;

        return scope.IsNullOrEmpty() ? applicationContext : $"{applicationContext}{contextSeparator}{scope}";
    }

    /// <summary>
    /// Sets the DomainId claim for the authenticated user in the provided authorization context.
    /// </summary>
    /// <param name="authUser">The authenticated user.</param>
    /// <param name="context">The authorization context.</param>
    private static void SetUserDomainId(AuthUser authUser, AuthorizationHandlerContext context)
    {
        context.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Sid, authUser.DomainId) }));
    }

    /// <summary>
    /// Checks if the user is authenticated based on the presence of a valid user identity in the authorization context.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <returns>True if the user is authenticated; otherwise, false.</returns>
    private static bool IsUserAuthenticated(AuthorizationHandlerContext context)
    {
        return context?.User.Identity is not null && context.User.Identity.IsAuthenticated;
    }

    /// <summary>
    /// Logs a warning message and marks the provided authorization context as failed.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="reason">The reason for the failure.</param>
    private void Fail(AuthorizationHandlerContext context, string reason)
    {
        logger.Warning(reason);
        context.Fail();
    }

    /// <summary>
    /// Extracts a scope from the application context using a specified separator.
    /// </summary>
    /// <param name="appContext">The application context.</param>
    /// <returns>The extracted scope or the original context if the separator is not present.</returns>
    private string ExtractScope(string appContext)
    {
        return !appContext.Contains(contextSeparator) ? appContext : appContext.Split(contextSeparator)[1];
    }

    /// <summary>
    /// Handles the authorization requirement to check user roles.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The authorization requirement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        RolesAuthorizationRequirement requirement)
    {
        if (!IsUserAuthenticated(context) || requirement is null)
        {
            Fail(context, "User not authenticated.");
            return;
        }

        var userId = GetUserId(context);

        if (requirement.AllowedRoles?.Any() == false || userId.IsNullOrEmpty())
        {
            Fail(context, $"Invalid user roles or id.The current user id is {userId}.");
            return;
        }

        var user = await securityRepository.GetUserByExternalId(userId, CancellationToken.None).ConfigureAwait(false);

        if (user is null)
        {
            Fail(context, $"User of id {userId} not found for role authorization.");
            return;
        }

        var roles = GetUserRoles(user);

        if (roles is null)
        {
            Fail(context, $"User of id {userId} has no roles defined.");
            return;
        }

        var appContext = GetApplicationContext(context);
        var scope = ExtractScope(appContext);

        var hasPermission = appContext.IsNullOrEmpty()
            ? roles.Any(r => requirement.AllowedRoles.Contains(r.Name, StringComparer.OrdinalIgnoreCase))
            : roles.Any(r => (r.Scope == appContext || r.Scope == "*" || r.Scope == $"*::{scope}") &&
                             requirement.AllowedRoles.Contains(r.Name, StringComparer.OrdinalIgnoreCase));

        if (hasPermission)
        {
            SetUserDomainId(user, context);

            context.Succeed(requirement);
        }
        else
        {
            Fail(context, $"User of id {userId} has no roles defined.");
            context.Fail();
        }
    }

    /// <summary>
    /// Gets the combined list of roles associated with the specified authenticated user.
    /// This includes both individual roles assigned to the user and roles associated with the user's groups.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>A list of roles associated with the user.</returns>
    private static List<Role> GetUserRoles(AuthUser user)
    {
        var roles = new List<Role>();

        if (user.Roles is not null)
            roles.AddRange(user.Roles);

        var groupRoles = user.Groups?.SelectMany(g => g.Roles).ToList();

        if (groupRoles is not null)
            roles.AddRange(groupRoles);

        return roles;
    }
}