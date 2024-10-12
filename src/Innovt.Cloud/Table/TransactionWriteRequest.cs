// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table;

/// <summary>
///     Represents a request for a transactional write operation.
/// </summary>
public class TransactionWriteRequest
{
    /// <summary>
    ///     Default constructor for TransactionWriteRequest.
    /// </summary>
    public TransactionWriteRequest()
    {
        TransactItems = new List<TransactionWriteItem>();
    }

    /// <summary>
    ///     Gets or sets a client-provided request token to ensure idempotent execution of the transaction.
    /// </summary>
    public string ClientRequestToken { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    /// <summary>
    ///     Gets or sets a list of transactional write items to be included in the transaction.
    /// </summary>
    public IList<TransactionWriteItem> TransactItems { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <summary>
    ///     Add a new item to the transaction
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(TransactionWriteItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        TransactItems.Add(item);
    }
}