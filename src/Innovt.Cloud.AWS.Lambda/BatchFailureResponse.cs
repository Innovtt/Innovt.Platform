using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda
{
    public class BatchFailureResponse
    {
        public BatchFailureResponse()
        {
        }

        [JsonPropertyName("batchItemFailures")]
        public List<ItemFailureIdentifier> BatchItemFailures { get; private set; }

        public void AddItem(string itemIdentifier) {

            BatchItemFailures ??= new List<ItemFailureIdentifier>();

            if (BatchItemFailures.Any(i => i.ItemIdentifier == itemIdentifier))
                return;

            BatchItemFailures.Add(new ItemFailureIdentifier(itemIdentifier));
        }

        public void AddItems(IEnumerable<string> itemsIdentifier)
        {
            if (itemsIdentifier == null)
                return;

            foreach (string item in itemsIdentifier)
            {
                AddItem(item);
            }            
        }
    }
}
