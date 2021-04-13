// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public interface IGetUserResponse
    {
        string UserName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Status { get; set; }

        DateTime UserCreateDate { get; set; }

        DateTime UserLastModifiedDate { get; set; }
    }
}