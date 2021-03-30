using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class SecurityGroup : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public IList<SecurityGroupPolicy> Policies { get; set; }

        public IList<SecurityGroupUser> Users { get; set; }

        public SecurityGroup()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }


        public IList<Policy> GetPolicies()
        {
            return Policies?.Select(p => p.Policy).ToList();
        }


        public void LinkPolicy(int policyId)
        {
            if (Policies.IsNullOrEmpty())
                Policies = new List<SecurityGroupPolicy>();

            var exist = Policies.Any(p => p.PolicyId == policyId);

            if (exist)
                throw new BusinessException($"Policy already linked for this security group.");

            var sgPolicy = new SecurityGroupPolicy {SecurityGroup = this, PolicyId = policyId};

            Policies.Add(sgPolicy);
        }

        public void UnlinkPolicy(int policyId)
        {
            var sGpolicy = Policies?.FirstOrDefault(p => p.PolicyId == policyId);

            if (sGpolicy == null)
                throw new BusinessException($"This Policy is not linked.");

            Policies.Remove(sGpolicy);
        }


        public void AddUser(int userId)
        {
            if (Users.IsNullOrEmpty())
                Users = new List<SecurityGroupUser>();

            var exist = Users.Any(p => p.UserId == userId);

            if (exist)
                throw new BusinessException($"User {userId} already allow to this group.");

            var userGroup = new SecurityGroupUser {SecurityGroup = this, UserId = userId};

            Users.Add(userGroup);
        }

        public void RemoveUser(int userId)
        {
            var userGroup = Users?.FirstOrDefault(p => p.UserId == userId);

            if (userGroup == null)
                throw new BusinessException($"User {userId} do not allow to this Security Group.");

            Users.Remove(userGroup);
        }
    }
}