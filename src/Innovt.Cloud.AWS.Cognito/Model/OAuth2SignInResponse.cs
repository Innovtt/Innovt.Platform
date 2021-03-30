using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class OAuth2SignInResponse : SignInResponse
    {
        [JsonPropertyName("error")] public string Error { get; set; }

        public bool NeedRegister { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        public string Email { get; set; }
    }
}