using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda
{
    public sealed class BatchFailureResponse
    {
        [JsonPropertyName("itemIdentifier")]
        public string ItemIdentifier { get; set; }

        public BatchFailureResponse(string itemIdentifier)
        {
            ItemIdentifier = itemIdentifier;
        }

        public BatchFailureResponse()
        {

        }
    }
}
