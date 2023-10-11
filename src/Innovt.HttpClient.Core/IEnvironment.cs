using System;

namespace Innovt.HttpClient.Core;

/// <summary>
/// Interface representing environment configurations.
/// </summary>
public interface IEnvironment
{
    /// <summary>
    /// Gets the URI for transactions.
    /// </summary>
    Uri TransactionUri { get; }

    /// <summary>
    /// Gets the URI for queries.
    /// </summary>
    Uri QueryUri { get; }
}