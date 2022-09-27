// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;

public class TransactionWriteRequest
{
    public TransactionWriteRequest()
    {
        TransactItems = new List<TransactionWriteItem>();
    }

    public string ClientRequestToken { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    public IList<TransactionWriteItem> TransactItems { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}