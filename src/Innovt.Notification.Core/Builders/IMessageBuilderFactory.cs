// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Builders;

/// <summary>
///     Interface for message builder factories.
/// </summary>
public interface IMessageBuilderFactory
{
    /// <summary>
    ///     Creates an instance of a message builder based on the provided builder name.
    /// </summary>
    /// <param name="builderName">The name of the builder.</param>
    /// <returns>The message builder instance.</returns>
    IMessageBuilder Create(string builderName);
}