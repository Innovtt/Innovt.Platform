// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;
using System;

namespace Innovt.Domain.Users
{
    public class BaseUser : Entity
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual bool IsActive { get; set; }

        public DateTimeOffset? LastAccess { get; set; }

        public string Name => $"{FirstName} {LastName}";
    }
}