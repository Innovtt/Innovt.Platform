using System.Security.Claims;
using Innovt.Domain.Security;
using Innovt.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Innovt.AspNetCore.Infrastructure;

/// <summary>
/// This is a service that retrieves the current user from the HttpContext.
/// It implements the IContextUserService interface.
/// </summary>
/// <param name="httpContextAccessor"></param>
public class ContextUserService(IHttpContextAccessor httpContextAccessor) : IContextUserService
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor ??
                                                                throw new ArgumentNullException(
                                                                    nameof(httpContextAccessor));

    /// <summary>
    /// returns the current user from the HttpContext. Only works if the user is authenticated.
    /// </summary>
    /// <returns></returns>
    public ContextUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null) return null;
        if (!user.Identity?.IsAuthenticated ?? true) return null;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var email = user.FindFirstValue(ClaimTypes.Email);
        var name = user.FindFirstValue(ClaimTypes.Name);

        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role).Select(n => n.Value).ToList();

        var claims = user.Claims
            .Where(c => c.Type != ClaimTypes.NameIdentifier && c.Type != ClaimTypes.Email && c.Type != ClaimTypes.Name
                        && c.Type != ClaimTypes.Role)
            .ToDictionary(c => c.Type, c => c.Value);

        var contextUser = new ContextUser();

        contextUser.SetId(userId).SetEmail(email).SetName(name)
            .SetRoles(roles)
            .SetClaims(claims);

        return contextUser;
    }
}