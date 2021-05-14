using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Authorization.Platform.Application.Commands;

namespace Innovt.Authorization.Platform.Application
{
    public interface ISecurityAppService
    {
        Task<IList<PermissionDTO>> FindPermissionBy(string domain = null, string resource = null, string name = null);

        Task AddPermission(AddPermissionCommand command);

        Task AddPolicie(AddPolicyCommand command);

        Task RemovePermission(RemovePermissionCommand permission);

        Task RemovePolicy(RemovePolicyCommand policy);

        Task<IList<PermissionDTO>> GetPermissionsBy(string domain = null, string resource = null, string name = null);

        Task<IList<PolicyDTO>> GetPolicies(string name = null, string description = null);

        Task<IList<PermissionDTO>> GetUserPermissions(string userId, string domain = null, string resource = null);
    }
}
