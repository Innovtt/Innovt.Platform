// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using System.Threading;
using System.Threading.Tasks;
using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core;
/// <summary>
/// Interface for a notification handler.
/// </summary>
public interface INotificationHandler
{
    /// <summary>
    /// Sends a notification message asynchronously.
    /// </summary>
    /// <param name="message">The notification message to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and returning a dynamic result.</returns>
    Task<dynamic> SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}