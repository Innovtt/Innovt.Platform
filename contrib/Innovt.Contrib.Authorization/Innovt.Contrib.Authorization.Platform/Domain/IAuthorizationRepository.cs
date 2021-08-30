using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public interface IAuthorizationRepository: Innovt.Domain.Security.IAuthorizationRepository
    {
        Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken);

        Task Save(AdminUser userFilter, CancellationToken cancellationToken);
    }
}
