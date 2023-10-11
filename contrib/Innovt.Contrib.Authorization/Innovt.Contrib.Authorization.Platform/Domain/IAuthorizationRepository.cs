// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Domain;

/// <summary>
/// Represents a repository for handling authorization-related operations.
/// </summary>
public interface IAuthorizationRepository : Innovt.Domain.Security.IAuthorizationRepository
{
    /// <summary>
    /// Saves an administrator user asynchronously.
    /// </summary>
    /// <param name="adminUser">The administrator user to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Save(AdminUser adminUser, CancellationToken cancellationToken);

    /// <summary>
    /// Saves a user asynchronously.
    /// </summary>
    /// <param name="user">The user to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Save(AuthUser user, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a user asynchronously.
    /// </summary>
    /// <param name="user">The user to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveUser(AuthUser user, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an administrator user based on the provided user filter asynchronously.
    /// </summary>
    /// <param name="userFilter">The user filter.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The administrator user.</returns>
    Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the roles associated with a user based on the provided filter asynchronously.
    /// </summary>
    /// <param name="filter">The filter for retrieving roles by user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of roles.</returns>
    Task<IList<Role>> GetUserRolesBy(RoleByUserFilter filter, CancellationToken cancellationToken);
}