// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;

namespace Innovt.Contrib.Authorization.Platform.Application;
/// <summary>
/// Interface representing the application service responsible for handling authorization-related operations.
/// </summary>
public interface IAuthorizationAppService
{
    /// <summary>
    /// Adds a user using the provided add user command.
    /// </summary>
    /// <param name="command">The add user command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddUser(AddUserCommand command, CancellationToken cancellationToken);
    /// <summary>
    /// Gets the roles associated with a user based on the provided filter.
    /// </summary>
    /// <param name="filter">The filter for retrieving roles by user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of role DTOs.</returns>
    Task<IList<RoleDto>> GetUserRoles(RoleByUserFilter filter, CancellationToken cancellationToken);
    /// <summary>
    /// Removes a user using the provided remove user command.
    /// </summary>
    /// <param name="command">The remove user command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveUser(RemoveUserCommand command, CancellationToken cancellationToken);
    /// <summary>
    /// Assigns roles to a user using the provided assign role command.
    /// </summary>
    /// <param name="command">The assign role command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AssignRole(AssignRoleCommand command, CancellationToken cancellationToken);
    /// <summary>
    /// Unassigns roles from a user using the provided unassign role command.
    /// </summary>
    /// <param name="command">The unassign role command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UnAssignRole(UnAssignUserRoleCommand command, CancellationToken cancellationToken);
    /// <summary>
    /// Registers an administrator using the provided registration command.
    /// </summary>
    /// <param name="command">The registration command for the administrator.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RegisterAdmin(RegisterAdminCommand command, CancellationToken cancellationToken);
}