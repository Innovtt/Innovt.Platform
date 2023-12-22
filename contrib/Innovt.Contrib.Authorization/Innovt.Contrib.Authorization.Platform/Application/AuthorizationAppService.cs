// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Core.Collections;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Innovt.Core.Validation;
using Innovt.Domain.Security;
using IAuthorizationRepository = Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationRepository;

namespace Innovt.Contrib.Authorization.Platform.Application;

/// <summary>
///     Application service responsible for handling authorization-related operations.
/// </summary>
public class AuthorizationAppService : IAuthorizationAppService
{
    private readonly IAuthorizationRepository authorizationRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthorizationAppService" /> class.
    /// </summary>
    /// <param name="authorizationRepository">The authorization repository.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="authorizationRepository" /> is null.</exception>
    public AuthorizationAppService(IAuthorizationRepository authorizationRepository)
    {
        this.authorizationRepository = authorizationRepository ??
                                       throw new ArgumentNullException(nameof(authorizationRepository));
    }

    /// <summary>
    ///     Registers an administrator using the provided registration command.
    /// </summary>
    /// <param name="command">The registration command for the administrator.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="BusinessException">Thrown when the user or password is invalid.</exception>
    public async Task RegisterAdmin(RegisterAdminCommand command, CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        var adminUser = await authorizationRepository.GetAdminUser(new UserFilter(command.Email), cancellationToken)
            .ConfigureAwait(false);

        if (adminUser != null && command.Password.Md5Hash() != adminUser.PasswordHash)
            throw new BusinessException(Messages.InvalidUserOrPassword);


        adminUser ??= new AdminUser
        {
            Email = command.Email,
            IsEnabled = true,
            Name = command.Name,
            PasswordHash = command.Password.Md5Hash()
        };

        adminUser.RegisterAccess();

        await authorizationRepository.Save(adminUser, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Adds a user using the provided add user command.
    /// </summary>
    /// <param name="command">The add user command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="BusinessException">Thrown when the user already exists.</exception>
    public async Task AddUser(AddUserCommand command, CancellationToken cancellationToken)
    {
        command.EnsureIsValid();

        var user = await authorizationRepository.GetUserByExternalId(command.Id, cancellationToken)
            .ConfigureAwait(false);

        if (user is not null)
            throw new BusinessException($"User {command.Id} already exist.");

        user = new AuthUser
        {
            Id = command.Id,
            DomainId = command.DomainId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        AssignRole(user, command.Roles);

        await authorizationRepository.Save(user, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Gets the roles associated with a user based on the provided filter.
    /// </summary>
    /// <param name="filter">The filter for retrieving roles by user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of role DTOs.</returns>
    public async Task<IList<RoleDto>> GetUserRoles(RoleByUserFilter filter, CancellationToken cancellationToken)
    {
        filter.EnsureIsValid();

        var roles = await authorizationRepository.GetUserRolesBy(filter, cancellationToken).ConfigureAwait(false);

        return RoleDto.FromDomain(roles);
    }

    /// <summary>
    ///     Removes a user using the provided remove user command.
    /// </summary>
    /// <param name="command">The remove user command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="BusinessException">Thrown when the user does not exist.</exception>
    public async Task RemoveUser(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        command.EnsureIsValid();

        var user = await authorizationRepository.GetUserByExternalId(command.Id, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
            throw new BusinessException($"User {command.Id} doesn't exist.");

        await authorizationRepository.RemoveUser(user, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Assigns roles to a user using the provided assign role command.
    /// </summary>
    /// <param name="command">The assign role command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="BusinessException">Thrown when the user does not exist.</exception>
    public async Task AssignRole(AssignRoleCommand command, CancellationToken cancellationToken)
    {
        command.EnsureIsValid();

        var user = await authorizationRepository.GetUserByExternalId(command.UserId, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
            throw new BusinessException($"User {command.UserId} doesn't exist.");

        AssignRole(user, command.Roles);

        await authorizationRepository.Save(user, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Unassigns roles from a user using the provided unassign role command.
    /// </summary>
    /// <param name="command">The unassign role command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="BusinessException">Thrown when the user does not exist.</exception>
    public async Task UnAssignRole(UnAssignUserRoleCommand command, CancellationToken cancellationToken)
    {
        command.EnsureIsValid();

        var user = await authorizationRepository.GetUserByExternalId(command.UserId, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
            throw new BusinessException($"User {command.UserId} doesn't exist.");

        foreach (var role in command.Roles) user.UnAssignRole(role.Scope, role.RoleName);

        await authorizationRepository.Save(user, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Assigns roles to a user based on the provided assign role command.
    /// </summary>
    /// <param name="user">The user to whom roles will be assigned.</param>
    /// <param name="roleCommands">The list of role commands to assign.</param>
    private static void AssignRole(AuthUser user, IList<AddRoleCommand> roleCommands)
    {
        if (roleCommands.IsNullOrEmpty())
            return;

        foreach (var role in roleCommands) user.AssignRole(new Role { Scope = role.Scope, Name = role.RoleName });
    }
}