﻿// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Security
{
    public class Group : Entity<Guid>
    {
        public Group()
        {
            CreatedAt = DateTimeOffset.UtcNow;
            Id = Guid.NewGuid();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Role> Roles { get; private set; }

        public void AssignRole(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            Roles ??= new List<Role>();

            var exist = Roles.Any(r => r.Id == role.Id);

            if (exist)
                throw new BusinessException($"Role {role.Id} already allow to this group.");

            Roles.Add(role);
        }

        public void UnAssignRole(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            var roleToRemove = Roles?.SingleOrDefault(r => r.Id == role.Id);

            if (roleToRemove is null)
                throw new BusinessException($"Role {role.Id} is not linked to this group.");

            Roles.Remove(role);
        }
    }
}