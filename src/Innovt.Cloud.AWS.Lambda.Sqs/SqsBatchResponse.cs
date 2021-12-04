using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Sqs
{
    public sealed class SqsBatchResponse
    {
        [JsonPropertyName("itemIdentifier")]
        public string ItemIdentifier { get; set; }

        public SqsBatchResponse(string itemIdentifier)
        {
            ItemIdentifier = itemIdentifier;
        }

        public SqsBatchResponse()
        {

        }
    }
}
