// Company: Antecipa
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-07-14

using System.Collections.Generic;

namespace Innovt.Cloud.Table
{
    public class TransactionWriteRequest
    {
        public string ClientRequestToken { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public IList<TransactionWriteItem> TransactItems { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    }
}