// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    /// <summary>
    /// Represents an aggregator of Roles
    /// </summary>
    public class Group : Entity
    {
        public Group()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public IList<Role> Roles { get; private set; }

        public IList<string> Users { get; private set; }


        public void AddRole(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            Roles ??= new List<Role>();

            var exist = Roles.Any(r => r.Id == role.Id);

            if (exist)
                throw new BusinessException($"Role {role.Id} already allow to this group.");
            
            Roles.Add(role);
        }


        public void RemoveRole(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            Roles ??= new List<Role>();

            var roleToRemove = Roles.SingleOrDefault(r => r.Id == role.Id);

            if (roleToRemove.IsNull())
                throw new BusinessException($"Role {role.Id} is not linked to this group.");

            Roles.Remove(role);
        }
        

        public void AddUser(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            
            Users ??= new List<string>();

            var exist = Users.Any(u => u== userId);

            if (exist)
                throw new BusinessException($"User {userId} already assigned to this group.");

            Users.Add(userId);
        }

        public void RemoveUser(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var user = Users?.SingleOrDefault(u => u == userId);

            if (user is null)
                throw new BusinessException($"User {userId} not assigned to this group.");

            Users.Remove(user);
        }
    }
}