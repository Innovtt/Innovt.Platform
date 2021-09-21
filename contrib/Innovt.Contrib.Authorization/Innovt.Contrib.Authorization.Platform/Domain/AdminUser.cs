// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Model;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public class AdminUser : Entity<Guid>
    {
        public AdminUser()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTimeOffset LastAccess { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsPasswordValid(string password)
        {
            return PasswordHash == password.Md5Hash();
        }

        public void RegisterAccess()
        {
            LastAccess = DateTime.UtcNow;
        }
    }
}