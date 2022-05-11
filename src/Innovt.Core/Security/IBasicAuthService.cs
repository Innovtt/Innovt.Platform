// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading.Tasks;

namespace Innovt.Core.Security;

public interface IBasicAuthService
{
    Task<bool> Authenticate(string userName, string password);
}