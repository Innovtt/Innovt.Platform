// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using Innovt.Contrib.Authorization.Platform.Domain;
using System;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class AdminUserDataModel : DataModelBase
    {
        public AdminUserDataModel()
        {
            EntityType = "AdminUser";
        }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastAccess { get; set; }

        public bool IsEnabled { get; set; }

        public static AdminUser ToUser(AdminUserDataModel userDataModel)
        {
            if (userDataModel is null)
                return null;

            return new AdminUser
            {
                Id = userDataModel.UserId,
                Name = userDataModel.Name,
                PasswordHash = userDataModel.PasswordHash,
                IsEnabled = userDataModel.IsEnabled,
                CreatedAt = userDataModel.CreatedAt,
                Email = userDataModel.Email,
                LastAccess = userDataModel.LastAccess
            };
        }

        public static AdminUserDataModel FromUser(AdminUser user)
        {
            if (user is null)
                return null;

            return new AdminUserDataModel
            {
                Id = $"MU#{user.Email}", //MU = masterUser
                Sk = "ADMINUSER",
                Name = user.Name,
                UserId = user.Id,
                CreatedAt = user.CreatedAt.GetValueOrDefault().UtcDateTime,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                IsEnabled = user.IsEnabled,
                LastAccess = user.LastAccess.UtcDateTime
            };
        }
    }
}