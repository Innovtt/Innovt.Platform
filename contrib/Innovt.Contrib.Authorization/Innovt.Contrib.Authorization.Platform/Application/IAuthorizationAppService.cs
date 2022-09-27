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

public interface IAuthorizationAppService
{
    Task AddUser(AddUserCommand command, CancellationToken cancellationToken);

    Task<IList<RoleDto>> GetUserRoles(RoleByUserFilter filter, CancellationToken cancellationToken);

    Task RemoveUser(RemoveUserCommand command, CancellationToken cancellationToken);

    Task AssignRole(AssignRoleCommand command, CancellationToken cancellationToken);

    Task UnAssignRole(UnAssignUserRoleCommand command, CancellationToken cancellationToken);

    Task RegisterAdmin(RegisterAdminCommand command, CancellationToken cancellationToken);
}