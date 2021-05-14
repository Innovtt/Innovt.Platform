using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Authorization.Platform.Application.Commands;
using Innovt.Domain.Security;

namespace Innovt.Authorization.Platform.Application
{
    public interface IAuthorizationAppService
    {
        Task AddPermission(AddPermissionCommand command);

        Task RemovePermission(Guid permissionId, CancellationToken cancellationToken);
        
        Task AddRole(AddRoleCommand role, CancellationToken cancellationToken);

        Task RemoveRole(Guid roleId, CancellationToken cancellationToken);


        //Task<IList<PermissionDTO>> FindPermissionBy(string domain = null, string resource = null, string name = null);

        

        //Task AddPolicie(AddPolicyCommand command);

        //Task RemovePermission(RemovePermissionCommand permission);

        //Task RemovePolicy(RemovePolicyCommand policy);

        //Task<IList<PermissionDTO>> GetPermissionsBy(string domain = null, string resource = null, string name = null);

        //Task<IList<PolicyDTO>> GetPolicies(string name = null, string description = null);

        //Task<IList<PermissionDTO>> GetUserPermissions(string userId, string domain = null, string resource = null);
    }
}
