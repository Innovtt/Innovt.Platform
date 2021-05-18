using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Authorization.Platform.Application.Commands;

namespace Innovt.Authorization.Platform.Application
{
    public interface IAuthorizationAppService
    {
        Task<Guid> AddPermission(AddPermissionCommand command, CancellationToken cancellationToken);

        Task RemovePermission(RemovePermissionCommand command, CancellationToken cancellationToken);
        
        Task<Guid> AddRole(AddRoleCommand command, CancellationToken cancellationToken);

        Task RemoveRole(RemoveRoleCommand command, CancellationToken cancellationToken);
        
        Task<Guid> AddGroup(AddGroupCommand command, CancellationToken cancellationToken);

        Task RemoveGroup(RemoveGroupCommand command, CancellationToken cancellationToken);
        
        Task Init(string moduleName,CancellationToken cancellationToken);

        //Task<IList<PermissionDTO>> FindPermissionBy(string domain = null, string resource = null, string name = null);

        //Task<IList<PermissionDTO>> GetUserPermissions(string userId, string domain = null, string resource = null);
    }
}
