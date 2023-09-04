// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud;
/// <summary>
///     Interface to be used for configuration
/// </summary>
public interface IConfiguration
{
    string SecretKey { get; set; }

    string AccessKey { get; set; }

    string Region { get; set; }
}