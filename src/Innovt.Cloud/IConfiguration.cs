// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;

namespace Innovt.Cloud;

/// <summary>
///     Interface to be used for configuration
/// </summary>
[CLSCompliant(true)]
public interface IConfiguration
{
    /// <summary>
    ///     Gets or sets the secret key for accessing the service.
    /// </summary>
    string SecretKey { get; set; }

    /// <summary>
    ///     Gets or sets the access key for accessing the service.
    /// </summary>
    string AccessKey { get; set; }

    /// <summary>
    ///     Gets or sets the region associated with the service.
    /// </summary>
    string Region { get; set; }
}