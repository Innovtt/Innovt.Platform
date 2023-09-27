using System;

namespace Innovt.HttpClient.Core
{
    public sealed class ApiContext
    {   
        public ApiContext(IEnvironment environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
        
        public ApiContext(IEnvironment environment, string accessToken):this(environment)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        public ApiContext(IEnvironment environment, BasicCredential credential):this(environment)
        {
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        public string AccessToken { get; set; }

        public BasicCredential Credential { get; set; }

        //"Bearer"
        public string TokenType { get; set; }

        public int ExpireIn { get; set; }
        public IEnvironment Environment { get; set; }
    }
}
