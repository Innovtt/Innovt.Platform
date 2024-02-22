// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.HealthChecks;

/// <summary>
///     Represents a service health check that can be used to monitor the status of a service.
/// </summary>
public interface IServiceHealthCheck
{
    /// <summary>
    ///     Gets or sets the name of the health check, which is used to identify the check.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     Performs the health check to determine the status of the associated service.
    /// </summary>
    /// <returns><c>true</c> if the service is healthy; otherwise, <c>false</c>.</returns>
    bool Check();
}