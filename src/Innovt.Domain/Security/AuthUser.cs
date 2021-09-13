// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{    
    public class AuthUser : Entity
    {
        public AuthUser()
        {
            CreatedAt = DateTimeOffset.UtcNow;               
        }

        public new string Id { get; set; }

        public string DomainId { get; set; }

        public string Name { get; set; }

        public IList<Permission> Permissions { get; set; }     
    }
}