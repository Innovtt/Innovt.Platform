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