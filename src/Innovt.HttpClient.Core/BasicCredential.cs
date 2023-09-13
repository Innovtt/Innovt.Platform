// Solution: PlugnPay.Acquirer.SDK
// Project: PlugnPay.Acquirer.SDK.Core
// User: Michel Magalhães
// Date: 2020/02/12 at 10:14 PM

using System;

namespace Innovt.HttpClient.Core
{
    public class BasicCredential
    {
        public string AccessKeyId { get; set; }

        public string AccessKey { get; set; }

        public BasicCredential(string accessKeyId, string accessKey)
        {
            AccessKeyId = accessKeyId ?? throw new ArgumentNullException(nameof(accessKeyId));
            AccessKey = accessKey ?? throw new ArgumentNullException(nameof(accessKey));
        }
    }
}