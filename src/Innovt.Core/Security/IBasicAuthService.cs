// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-05-07
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading.Tasks;

namespace Innovt.Core.Security
{
    public interface IBasicAuthService
    {
        Task<bool> Authenticate(string userName, string password);
    }
}