// Solution: PlugnPay.Acquirer.SDK
// Project: PlugnPay.Acquirer.SDK.Core
// User: Michel Magalhães
// Date: 2020/02/12 at 10:14 PM

using System;

namespace Innovt.HttpClient.Core;

/// <summary>
/// Represents basic credentials for authentication.
/// </summary>
public class BasicCredential
{
    /// <summary>
    /// Gets or sets the access key ID.
    /// </summary>
    public string AccessKeyId { get; set; }

    /// <summary>
    /// Gets or sets the access key.
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicCredential"/> class with the provided access key ID and access key.
    /// </summary>
    /// <param name="accessKeyId">The access key ID.</param>
    /// <param name="accessKey">The access key.</param>
    public BasicCredential(string accessKeyId, string accessKey)
    {
        AccessKeyId = accessKeyId ?? throw new ArgumentNullException(nameof(accessKeyId));
        AccessKey = accessKey ?? throw new ArgumentNullException(nameof(accessKey));
    }
}