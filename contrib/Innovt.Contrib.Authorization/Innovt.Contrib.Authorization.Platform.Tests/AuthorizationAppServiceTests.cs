// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform.Tests
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Core.Exceptions;
using Innovt.Domain.Security;
using NSubstitute;
using NUnit.Framework;
using IAuthorizationRepository = Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationRepository;

namespace Innovt.Contrib.Authorization.Platform.Tests
{
    public class AuthorizationAppServiceTests
    {
        private IAuthorizationAppService authorizationAppService;
        private IAuthorizationRepository authorizationRepositoryMock;

        [SetUp]
        public void Setup()
        {
            authorizationRepositoryMock = Substitute.For<IAuthorizationRepository>();

            authorizationAppService = new AuthorizationAppService(authorizationRepositoryMock);
        }


        [Test]
        public void AddUser_ThrowException_If_CommandIsNUll()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await authorizationAppService.AddUser(null, CancellationToken.None));
        }


        [Test]
        public void AddUser_ThrowException_When_Command_Is_Not_Valid()
        {
            var command = new AddUserCommand();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await authorizationAppService.AddUser(null, CancellationToken.None));
        }


        [Test]
        public void AddUser_ThrowException_When_Roles_Are_Invalid()
        {
            var userId = Guid.NewGuid();

            var actualUser = new AuthUser()
            {
                Id = userId.ToString(),
                DomainId = Guid.NewGuid().ToString(),
            };
            var command = new AddUserCommand()
            {
                Id = actualUser.Id,
                DomainId = actualUser.DomainId,
                Roles = new List<AddRoleCommand>() {
                new AddRoleCommand(){
                    Scope="",
                    RoleName="User"
                }
                }
            };
            
            var bex = Assert.ThrowsAsync<BusinessException>(async () => await authorizationAppService.AddUser(command, CancellationToken.None));

            Assert.IsNotNull(bex);
            Assert.AreEqual(bex.Errors.Count(), 1);
            Assert.AreEqual(bex.Errors.First().Message, "Scope is required.");
        }


        [Test]
        public void AddUser_ThrowException_When_UserAlready_Exist()
        {
            var userId = Guid.NewGuid();

            var actualUser = new AuthUser()
            {
                Id = userId.ToString(),
                DomainId = Guid.NewGuid().ToString(),
            };
            var command = new AddUserCommand()
            {
                Id = actualUser.Id,
                DomainId = actualUser.DomainId
            };

            authorizationRepositoryMock.GetUserByExternalId(userId.ToString()).Returns(actualUser);

            Assert.ThrowsAsync<BusinessException>(async () => await authorizationAppService.AddUser(command, CancellationToken.None),
                $"User {command.Id} already exist.");
        }

        [Test]
        public void AssignRole_ThrowException_If_CommandIsNUll()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await authorizationAppService.AssignRole(null, CancellationToken.None));
        }

        [Test]
        public void AssignRole_ThrowException_If_There_Is_NoRoles()
        {
            var command = new AssignRoleCommand()
            {
                UserId = "123465"
            };

             Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AssignRole(command, CancellationToken.None));
        }

        [Test]
        public void AssignRole_ThrowException_If_There_Is_An_Invalid_Roles()
        {
            var command = new AssignRoleCommand()
            {
                UserId = "123465",
                Roles = new List<AddRoleCommand>() {
                new AddRoleCommand(){
                    Scope="",
                    RoleName="User"
                }
                }
            };

            Assert.ThrowsAsync<BusinessException>(async () =>
               await authorizationAppService.AssignRole(command, CancellationToken.None));
        }
    }
}