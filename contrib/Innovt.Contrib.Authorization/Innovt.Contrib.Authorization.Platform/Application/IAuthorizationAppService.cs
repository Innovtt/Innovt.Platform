using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;

namespace Innovt.Contrib.Authorization.Platform.Application
{
    public interface IAuthorizationAppService
    {
        Task AddUser(AddUserCommand command, CancellationToken cancellationToken);

        Task RemoveUser(RemoveUserCommand command, CancellationToken cancellationToken);

        Task<Guid> AddPermission(AddPermissionCommand command, CancellationToken cancellationToken);

        Task RemovePermission(RemovePermissionCommand command, CancellationToken cancellationToken);

        Task<IList<PermissionDto>> FindPermissionBy(PermissionFilter filter, CancellationToken cancellationToken);
        

        Task<Guid> AddRole(AddRoleCommand command, CancellationToken cancellationToken);

        Task RemoveRole(RemoveRoleCommand command, CancellationToken cancellationToken);

        Task<IList<RoleDto>> FindRoleBy(RoleFilter filter, CancellationToken cancellationToken);

        Task<Guid> AddGroup(AddGroupCommand command, CancellationToken cancellationToken);

        Task RemoveGroup(RemoveGroupCommand command, CancellationToken cancellationToken);
        
        Task RegisterAdmin(RegisterAdminCommand command,CancellationToken cancellationToken);

        Task<IList<GroupDto>> FindGroupBy(GroupFilter filter, CancellationToken cancellationToken);
    }
}
