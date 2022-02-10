using Innovt.Domain.Security;
using NUnit.Framework;
using System.Linq;

namespace Innovt.Domain.Tests
{
    public class AuthUserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UnAssignRole()
        {
            var user = new AuthUser();

            var roleA = new Role()
            {
                Name = "RoleA",
                Scope = "Admin"
            };

            var roleB = new Role()
            {
                Name = "RoleB",
                Scope = "Admin"
            };

            var roleC = new Role()
            {
                Name = "RoleC",
                Scope = "Admin"
            };

            user.AssignRole(roleA);
            user.AssignRole(roleB);
            user.AssignRole(roleC);

            Assert.IsNotNull(user.Roles);
            Assert.IsTrue(user.Roles.Count == 3);

            user.UnAssignRole(roleB.Scope, roleB.Name);

            Assert.IsNotNull(user.Roles);
            Assert.IsTrue(user.Roles.Count == 2);

            Assert.IsTrue(user.Roles.Count(r => r.Name == roleA.Name) == 1);

            Assert.IsTrue(user.Roles.Count(r => r.Name == roleC.Name) == 1);
        }
    }
}