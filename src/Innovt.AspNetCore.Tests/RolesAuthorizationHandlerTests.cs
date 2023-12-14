// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore.Tests

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Innovt.AspNetCore.Handlers;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.AspNetCore.Tests;

[TestFixture]
public class RolesAuthorizationHandlerTests
{
    [SetUp]
    public void Setup()
    {
        authorizationRepositoryMoq = Substitute.For<IAuthorizationRepository>();
        loggerMock = Substitute.For<ILogger>();
    }

    [TearDown]
    public void TearDown()
    {
        authorizationRepositoryMoq = null;
        loggerMock = null;
    }

    private IAuthorizationRepository authorizationRepositoryMoq;
    private ILogger loggerMock;

    [Test]
    public void ConstructorShould_ThrowException_If_Parameters_Is_NUll()
    {
        Assert.Throws<ArgumentNullException>(() => new RolesAuthorizationHandler(null, loggerMock));

        Assert.Throws<ArgumentNullException>(() => new RolesAuthorizationHandler(authorizationRepositoryMoq, null));
    }

    private AuthorizationHandlerContext CreateContext(ClaimsPrincipal user)
    {
        return new AuthorizationHandlerContext(new List<IAuthorizationRequirement>
        {
            new RolesAuthorizationRequirement(new[] { "Admin" })
        }, user, null);
    }

    [Test]
    public async Task HandleAsync_Fail_If_User_Is_Not_Authenticated()
    {
        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var user = new ClaimsPrincipal();
        var context = CreateContext(user);
        await handle.HandleAsync(context);
        Assert.That(context.HasFailed, Is.True);
    }

    [Test]
    public async Task HandleAsync_Fail_If_User_HasNoId()
    {
        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);

        var principal = new ClaimsPrincipal(identity);
        var context = CreateContext(principal);
        await handle.HandleAsync(context);
        Assert.That(context.HasFailed, Is.True);
    }

    [Test]
    public async Task HandleAsync_Fail_If_User_Does_Not_Exist()
    {
        authorizationRepositoryMoq.GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(default(AuthUser));

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "564654654")
        });

        var principal = new ClaimsPrincipal(identity);

        var context = CreateContext(principal);

        await handle.HandleAsync(context);
        Assert.That(context.HasFailed, Is.True);

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task HandleAsync_Fail_If_User_HasNoRoles()
    {
        var user = new AuthUser
        {
            Name = "MIchel",
            DomainId = "123456",
            Id = "michel@antecipa.com"
        };

        authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        var context = CreateContext(principal);

        await handle.HandleAsync(context);

        Assert.That(context.HasFailed, Is.True);

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());

        loggerMock.Received(1).Warning($"User of id {user.Id} has no roles defined.");
    }

    [Test]
    [TestCase("Admin", true)]
    [TestCase("User", false)]
    public async Task HandleAsync_Fail_If_User_Has_NoMatting_Role(string role, bool success)
    {
        var group = new Group
        {
            Description = "Default"
        };

        group.AssignRole(new Role
        {
            Scope = "User", //no scope
            Name = role
        });

        var user = new AuthUser
        {
            Name = "Michel",
            DomainId = "123456",
            Id = "michel@antecipa.com"
        };

        user.AssignGroup(group);

        authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        var context = CreateContext(principal);

        await handle.HandleAsync(context);


        Assert.That(context.HasSucceeded, Is.EqualTo(success));
        Assert.That(context.HasFailed, Is.EqualTo(!success));

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task HandleAsync_Fail_If_User_Has_NoMatting_Scope()
    {
        var group = new Group
        {
            Description = "Default"
        };

        group.AssignRole(new Role
        {
            Scope = "Buyer", //Fixed scope
            Name = "Admin"
        });

        var user = new AuthUser
        {
            Name = "Michel",
            DomainId = "123456",
            Id = "michel@antecipa.com"
        };

        user.AssignGroup(group);

        authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-Application-Scope", "User");

        var context = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>
        {
            new RolesAuthorizationRequirement(new[] { "Admin" })
        }, principal, httpContext);


        await handle.HandleAsync(context);

        Assert.That(context.HasFailed, Is.True);

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }


    [Test]
    public async Task HandleAsync_Succeed_If_User_Has_Scope_And_Role()
    {
        var scope = "Buyer";
        var group = new Group
        {
            Description = "Default"
        };

        group.AssignRole(new Role
        {
            Scope = scope,
            Name = "Admin"
        });

        var user = new AuthUser
        {
            Name = "Michel",
            DomainId = "123456",
            Id = "michel@antecipa.com"
        };

        user.AssignGroup(group);

        authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-Application-Scope", scope);

        var context = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>
        {
            new RolesAuthorizationRequirement(new[] { "Admin" })
        }, principal, httpContext);

        await handle.HandleAsync(context);

        Assert.That(context.HasSucceeded, Is.True);
        Assert.That(context.HasFailed, Is.False);

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Test]
    [TestCase("Buyer", "123456", true)]
    [TestCase("User", "123456", false)]
    public async Task HandleAsync_When_User_Has_Scope_And_Role_And_ApplicationCode(string scope, string appCode,
        bool success)
    {
        var group = new Group
        {
            Description = "Default"
        };

        group.AssignRole(new Role
        {
            Scope = "123456::Buyer",
            Name = "Admin"
        });

        var user = new AuthUser
        {
            Name = "Michel",
            DomainId = "123456",
            Id = "michel@antecipa.com"
        };

        user.AssignGroup(group);

        authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-Application-Scope", scope);
        httpContext.Request.Headers.Append("X-Application-Context", "company-id");
        httpContext.Request.Headers.Append("company-id", appCode);

        var context = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>
        {
            new RolesAuthorizationRequirement(new[] { "Admin" })
        }, principal, httpContext);

        await handle.HandleAsync(context);

        Assert.That(context.HasSucceeded, Is.EqualTo(success));
        Assert.That(context.HasFailed, Is.EqualTo(!success));

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Test]
    [TestCase("*", "123456", true)]
    [TestCase("*::User", "123456", true)]
    [TestCase("User", "123456", false)]
    public async Task HandleAsync_When_User_Has_WildCard_Scope(string scope, string appCode, bool success)
    {
        var group = new Group
        {
            Description = "Default"
        };

        group.AssignRole(new Role
        {
            Scope = scope,
            Name = "Admin"
        });

        var user = new AuthUser
        {
            Name = "Michel",
            DomainId = "123456",
            Id = "michel@antecipa.com"
        };

        user.AssignGroup(group);

        authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

        var identity = Substitute.For<ClaimsIdentity>();

        identity.IsAuthenticated.Returns(true);
        identity.Claims.Returns(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-Application-Scope", "User");
        httpContext.Request.Headers.Append("X-Application-Context", "company-id");
        httpContext.Request.Headers.Append("company-id", appCode);

        var context = new AuthorizationHandlerContext(new List<IAuthorizationRequirement>
        {
            new RolesAuthorizationRequirement(new[] { "Admin" })
        }, principal, httpContext);

        await handle.HandleAsync(context);

        Assert.That(context.HasSucceeded, Is.EqualTo(success));
        Assert.That(context.HasFailed, Is.EqualTo(!success));

        await authorizationRepositoryMoq.Received(1)
            .GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}