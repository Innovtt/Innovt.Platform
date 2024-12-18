// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Core.Security;

/// <summary>
///     Represents an interface for basic authentication services.
/// </summary>
public interface IBasicAuthService
{
    /// <summary>
    ///     Authenticates a user based on a provided username and password.
    /// </summary>
    /// <param name="userName">The username to authenticate.</param>
    /// <param name="password">The password associated with the username.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operartion</param>
    /// <returns>
    ///     A task that represents the asynchronous authentication operation.
    ///     The task result is <c>true</c> if the authentication is successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> Authenticate(string userName, string password, CancellationToken cancellationToken = default);
}