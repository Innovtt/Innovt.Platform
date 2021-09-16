// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Security
{
    public interface IAuthorizationRepository
    {
        Task<AuthUser> GetUser(string userId, CancellationToken cancellationToken = default);
    }
}