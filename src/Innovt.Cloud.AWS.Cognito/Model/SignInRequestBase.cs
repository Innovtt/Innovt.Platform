// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model;

public abstract class SignInRequestBase : RequestBase
{
    [Required] public string UserName { get; set; }
}