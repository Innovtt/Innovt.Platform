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
using NSubstitute;
using NUnit.Framework;

namespace Innovt.AspNetCore.Tests
{
    public class RolesAuthorizationHandlerTests
    {
        private IAuthorizationRepository authorizationRepositoryMoq;
        private ILogger loggerMock;

        
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

        [Test]
        public void ConstructorShould_ThrowException_If_Parameters_Is_NUll()
        { 
            Assert.Throws<ArgumentNullException>(() => new RolesAuthorizationHandler(null, loggerMock));
            
            Assert.Throws<ArgumentNullException>(() => new RolesAuthorizationHandler(authorizationRepositoryMoq, null));
        }

        private AuthorizationHandlerContext CreateContext(ClaimsPrincipal user)
        {
            return  new AuthorizationHandlerContext(new List<IAuthorizationRequirement>()
            {
                new RolesAuthorizationRequirement(new []{"Admin"})
            }, user, null);
        }

        [Test]
        public async Task HandleAsync_Fail_If_User_Is_Not_Authenticated()
        {
            var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

            var user = new ClaimsPrincipal();
            var context = CreateContext(user);
            await handle.HandleAsync(context);
            Assert.IsTrue(context.HasFailed);
        }

        [Test]
        public async Task HandleAsync_Fail_If_User_HasNoId()
        {
            var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);
            
            var identity = NSubstitute.Substitute.For<ClaimsIdentity>();
            
            identity.IsAuthenticated.Returns(true);

            var principal = new ClaimsPrincipal(identity);
            var context = CreateContext(principal);
            await handle.HandleAsync(context);
            Assert.IsTrue(context.HasFailed);
        }
        
        [Test]
        public async Task HandleAsync_Fail_If_User_Does_Not_Exist()
        {
            authorizationRepositoryMoq.GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(default(AuthUser));

            var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

            var identity = NSubstitute.Substitute.For<ClaimsIdentity>();

            identity.IsAuthenticated.Returns(true);
            identity.Claims.Returns(new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, "564654654")
            });

            var principal = new ClaimsPrincipal(identity);

            var context = CreateContext(principal);
            
            await handle.HandleAsync(context);
            Assert.IsTrue(context.HasFailed);

            await authorizationRepositoryMoq.Received(1).GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
        }
        
        [Test]
        public async Task HandleAsync_Fail_If_User_HasNoRoles()
        {
            var user = new AuthUser()
            {
                Name = "MIchel",
                DomainId = "123456",
                Id = "michel@antecipa.com"
            };

            authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
                .Returns(user);

            var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

            var identity = NSubstitute.Substitute.For<ClaimsIdentity>();

            identity.IsAuthenticated.Returns(true);
            identity.Claims.Returns(new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id)
            });

            var principal = new ClaimsPrincipal(identity);

            var context = CreateContext(principal);

            await handle.HandleAsync(context);

            Assert.IsTrue(context.HasFailed);

            await authorizationRepositoryMoq.Received(1).GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());

            loggerMock.Received(1).Warning($"User of id {user.Id} has no roles defined.");
        }

        [Test]
        [TestCase("Admin", false)]
        [TestCase("User", true)]
        public async Task HandleAsync_Fail_If_User_Has_NoMatting_Role(string role, bool success)
        {
            var group = new Group()
            {
                Description = "Default"
            };

            group.AssignRole(new Role()
            {
                Scope = "default",
                Name = role
            });

            var user = new AuthUser()
            {
                Name = "Michel",
                DomainId = "123456",
                Id = "michel@antecipa.com"
            };

            user.AssignGroup(group);

            authorizationRepositoryMoq.GetUserByExternalId(user.Id, Arg.Any<CancellationToken>())
                .Returns(user);

            var handle = new RolesAuthorizationHandler(authorizationRepositoryMoq, loggerMock);

            var identity = NSubstitute.Substitute.For<ClaimsIdentity>();

            identity.IsAuthenticated.Returns(true);
            identity.Claims.Returns(new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id)
            });

            var principal = new ClaimsPrincipal(identity);

            var context = CreateContext(principal);

            await handle.HandleAsync(context);

            Assert.IsTrue(context.HasFailed == success);

            await authorizationRepositoryMoq.Received(1).GetUserByExternalId(Arg.Any<string>(), Arg.Any<CancellationToken>());
        }


    }
}