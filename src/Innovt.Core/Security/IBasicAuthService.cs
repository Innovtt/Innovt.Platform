// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Threading.Tasks;

namespace Innovt.Core.Security;

public interface IBasicAuthService
{
    Task<bool> Authenticate(string userName, string password);
}