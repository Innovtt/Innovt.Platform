// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-08-07

using System;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class UserDataModel : DataModelBase
    {
        public UserDataModel()
        {
            EntityType = "User";
        }

        public string AuthId { get; set; }
        public string DomainId { get; set; }

        public DateTime CreatedAt { get; set; }


        public static AuthUser ToUser(UserDataModel userDataModel)
        {
            if (userDataModel is null)
                return null;

            return new AuthUser
            {
                Id = userDataModel.AuthId,
                DomainId = userDataModel.DomainId,
                CreatedAt = userDataModel.CreatedAt
            };
        }

        public static UserDataModel FromUser(AuthUser user)
        {
            if (user is null)
                return null;

            return new UserDataModel
            {
                Id = $"U#{user.Id}",
                Sk = $"DID#{user.DomainId}",
                AuthId = user.Id,
                DomainId = user.DomainId,
                CreatedAt = user.CreatedAt.GetValueOrDefault().UtcDateTime,
            };
        }
    }
}