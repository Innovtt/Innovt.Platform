using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public abstract class RequestBase: IRequestBase
    {
        [JsonIgnore]
        public Dictionary<string,string> HttpHeader { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }

        [JsonIgnore]
        public string ServerPath { get; set; }

        [JsonIgnore]
        public string ServerName { get; set; }
        
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
