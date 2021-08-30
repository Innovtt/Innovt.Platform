using Innovt.Core.Utilities;
using Innovt.Domain.Core.Model;
using System;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public class AdminUser:Entity<Guid>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTimeOffset LastAccess { get; set; }

        public bool IsEnabled { get; set; }

        public AdminUser()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.UtcNow;
        }

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
