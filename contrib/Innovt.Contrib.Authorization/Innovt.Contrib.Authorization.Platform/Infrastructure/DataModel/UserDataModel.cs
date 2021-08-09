// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Security;
using Innovt.Domain.Users;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class UserDataModel : DataModelBase
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsEnabled { get; set; }

        public UserDataModel()
        {

        }
        
        public static BaseUser ToUser(UserDataModel userDataModel)
        {
            if (userDataModel is null)
                return null;

            return new BaseUser()
            {
                Id = userDataModel.UserId,
                FirstName  =  userDataModel.Name,
                Password   = userDataModel.Password,
                IsActive   = userDataModel.IsEnabled,
                CreatedAt  = userDataModel.CreatedAt,
                Email      = userDataModel.Email                
            };
        }

        public static UserDataModel FromUser(BaseUser user)
        {
            if (user is null)
                return null;

            return new UserDataModel()
            {
                Id = $"U#{user.Email}",
                Sk = user.Name,
                Name = user.Name,                
                CreatedAt = user.CreatedAt.GetValueOrDefault().UtcDateTime, 
                Email = user.Email,
                IsEnabled = user.IsActive,
                
                
            };
        }
    }
}