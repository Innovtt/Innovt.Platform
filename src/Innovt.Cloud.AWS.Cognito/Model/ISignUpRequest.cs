// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public interface ISignUpRequest : IRequestBase
    {
        string UserName { get; set; }

        string Password { get; set; }

        public Dictionary<string, string> CustomAttributes { get; set; }
    }
}