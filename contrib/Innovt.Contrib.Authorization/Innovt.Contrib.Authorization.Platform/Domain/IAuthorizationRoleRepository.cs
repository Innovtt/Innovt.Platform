// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-17

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public interface IAuthorizationRoleRepository : IAuthorizationRepository
    {
        Task AddRole(Role role, CancellationToken cancellationToken = default);

        Task RemoveRole(Role role, CancellationToken cancellationToken = default);

        Task<Role> GetRoleById(Guid roleId, CancellationToken cancellationToken = default);

        Task AddGroup(Group group, CancellationToken cancellationToken = default);

        Task<Group> GetGroupById(Guid groupId, CancellationToken cancellationToken = default);
    }
}