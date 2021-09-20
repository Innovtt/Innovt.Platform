// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-19

using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application
{
    public interface IAuthorizationAppService
    {
        Task AddUser(AddUserCommand command, CancellationToken cancellationToken);

        Task RemoveUser(RemoveUserCommand command, CancellationToken cancellationToken);

        Task AssignRole(AssignRoleCommand command, CancellationToken cancellationToken);

        Task UnAssignRole(UnAssignUserRoleCommand command, CancellationToken cancellationToken);

        Task RegisterAdmin(RegisterAdminCommand command, CancellationToken cancellationToken);
    }
}