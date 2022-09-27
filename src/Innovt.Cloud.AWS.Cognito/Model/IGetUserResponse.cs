// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

public interface IGetUserResponse
{
    string UserName { get; set; }

    string FirstName { get; set; }

    string LastName { get; set; }

    string Status { get; set; }

    Dictionary<string, string> CustomAttributes { get; set; }

    DateTime UserCreateDate { get; set; }

    DateTime UserLastModifiedDate { get; set; }
}