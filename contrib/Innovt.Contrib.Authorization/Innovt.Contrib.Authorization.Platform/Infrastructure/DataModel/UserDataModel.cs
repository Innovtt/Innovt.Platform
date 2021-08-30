// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Contrib.Authorization.Platform.Domain;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class UserDataModel : DataModelBase
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastAccess { get; set; }

        public bool IsEnabled { get; set; }

        public UserDataModel()
        {
            EntityType = "Admin";
        }
        
        public static AdminUser ToUser(UserDataModel userDataModel)
        {
            if (userDataModel is null)
                return null;

            return new AdminUser()
            {
                Id = userDataModel.UserId,
                Name  =  userDataModel.Name,
                PasswordHash   = userDataModel.PasswordHash,
                IsEnabled   = userDataModel.IsEnabled,
                CreatedAt  = userDataModel.CreatedAt,
                Email      = userDataModel.Email,
                LastAccess = userDataModel.LastAccess
            };
        }

        public static UserDataModel FromUser(AdminUser user)
        {
            if (user is null)
                return null;

            return new UserDataModel()
            {
                Id = $"MU#{user.Email}",//MU = masterUser
                Sk = "ADMINUSER",
                Name = user.Name,
                UserId =  user.Id,
                CreatedAt = user.CreatedAt.GetValueOrDefault().UtcDateTime, 
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                IsEnabled = user.IsEnabled,
                LastAccess = user.LastAccess.UtcDateTime
            };
        }
    }
}