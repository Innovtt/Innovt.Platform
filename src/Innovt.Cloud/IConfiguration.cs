// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud;

/// <summary>
///     Interface to be used for configuration
/// </summary>
public interface IConfiguration
{
    /// <summary>
    ///     Gets or sets the secret key for accessing the service.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    ///     Gets or sets the access key for accessing the service.
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    ///     Gets or sets the region associated with the service.
    /// </summary>
    public string Region { get; set; }
}