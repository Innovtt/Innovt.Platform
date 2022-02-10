// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Core.Collections;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Innovt.Core.Validation;
using Innovt.Domain.Security;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IAuthorizationRepository = Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationRepository;

namespace Innovt.Contrib.Authorization.Platform.Application
{
    public class AuthorizationAppService : IAuthorizationAppService
    {
        private readonly IAuthorizationRepository authorizationRepository;

        public AuthorizationAppService(IAuthorizationRepository authorizationRepository)
        {
            this.authorizationRepository = authorizationRepository ??
                                           throw new ArgumentNullException(nameof(authorizationRepository));
        }

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
        private static void AssignRole(AuthUser user, IList<AddRoleCommand> roleCommands)
        {
            if (roleCommands.IsNullOrEmpty())
                return;

            foreach (var role in roleCommands)
            {
                user.AssignRole(new Role { Scope = role.Scope, Name = role.RoleName });
            }
        }

        public async Task AddUser(AddUserCommand command, CancellationToken cancellationToken)
        {
            command.EnsureIsValid();

            var user = await authorizationRepository.GetUserByExternalId(command.Id, cancellationToken)
                .ConfigureAwait(false);

            if (user is { })
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

        public async Task<IList<RoleDto>> GetUserRoles(RoleByUserFilter filter, CancellationToken cancellationToken)
        {
            filter.EnsureIsValid();

            var roles = await authorizationRepository.GetUserRolesBy(filter, cancellationToken).ConfigureAwait(false);

            return RoleDto.FromDomain(roles);
        }

        public async Task RemoveUser(RemoveUserCommand command, CancellationToken cancellationToken)
        {
            command.EnsureIsValid();

            var user = await authorizationRepository.GetUserByExternalId(command.Id, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
                throw new BusinessException($"User {command.Id} doesn't exist.");

            await authorizationRepository.RemoveUser(user, cancellationToken).ConfigureAwait(false);
        }

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

        public async Task UnAssignRole(UnAssignUserRoleCommand command, CancellationToken cancellationToken)
        {
            command.EnsureIsValid();

            var user = await authorizationRepository.GetUserByExternalId(command.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
                throw new BusinessException($"User {command.UserId} doesn't exist.");

            foreach (var role in command.Roles)
            {
                user.UnAssignRole(role.Scope, role.RoleName);
            }

            await authorizationRepository.Save(user, cancellationToken).ConfigureAwait(false);
        }
    }
}