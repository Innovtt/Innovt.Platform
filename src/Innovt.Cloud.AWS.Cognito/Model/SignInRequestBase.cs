using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public abstract class SignInRequestBase : RequestBase
    {
        [Required] public string UserName { get; set; }
    }
}