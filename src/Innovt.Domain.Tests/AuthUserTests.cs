// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Tests

using System.Linq;
using Innovt.Domain.Security;
using NUnit.Framework;

namespace Innovt.Domain.Tests;

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

        var roleA = new Role
        {
            Name = "RoleA",
            Scope = "Admin"
        };

        var roleB = new Role
        {
            Name = "RoleB",
            Scope = "Admin"
        };

        var roleC = new Role
        {
            Name = "RoleC",
            Scope = "Admin"
        };

        user.AssignRole(roleA);
        user.AssignRole(roleB);
        user.AssignRole(roleC);

        Assert.That(user.Roles,Is.Not.Null);
        Assert.That(user.Roles, Has.Count.EqualTo(3));

        user.UnAssignRole(roleB.Scope, roleB.Name);

        Assert.That(user.Roles,Is.Not.Null);
        Assert.That(user.Roles, Has.Count.EqualTo(2));

        Assert.That(user.Roles.Count(r => r.Name == roleA.Name), Is.EqualTo(1));

        Assert.That(user.Roles.Count(r => r.Name == roleC.Name) , Is.EqualTo(1));
    }
}