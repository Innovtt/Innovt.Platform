// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform.Tests
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Infrastructure;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Domain.Security;
using NSubstitute;
using NUnit.Framework;
using IAuthorizationRepository = Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationRepository;

namespace Innovt.Contrib.Authorization.Platform.Tests
{
    [TestFixture]
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

        [Test]
        public async Task  AssignRole_ThrowException_If_There_Is_An_Invalid_RolesTest()
        {

            //var user = new AddUserCommand()
            //{
            //    DomainId = "88c2a0e6-d291-4a5c-a441-f7b25c9a944a",
            //    Id = "1fd52fa2-3837-4260-a8cc-a5f7c4c002b2",
            //    Roles = new List<AddRoleCommand>() {
            //    new AddRoleCommand(){
            //        Scope="901ac82d-7105-40f4-a5b4-b6954f6dbcd6::CapitalSource",
            //        RoleName="Admin",
            //    },
            //     new AddRoleCommand(){
            //        Scope="Antecipa",
            //        RoleName="Basic",
            //    }
            //    }
            //};

   

            var logger = NSubstitute.Substitute.For<ILogger>();

            var authorizationRepository = new AuthorizationRepository(logger, new DefaultAWSConfiguration("antecipa-prod"));

            authorizationAppService = new AuthorizationAppService(authorizationRepository);

           //await authorizationAppService.AddUser(user, CancellationToken.None);
          //var             await authorizationAppService.AssignRole(command, CancellationToken.None);           

            var roles = await authorizationAppService.GetUserRoles(new Domain.Filters.RoleByUserFilter()
            {
                DomainId = "88c2a0e6-d291-4a5c-a441-f7b25c9a944a",
                ExternalId = "1fd52fa2-3837-4260-a8cc-a5f7c4c002b2"
            }, CancellationToken.None);

            Console.Write(roles);

            //await authorizationAppService.AssignRole(command, CancellationToken.None));


        }



        [Test]
        public void UnAssignUserRole_ThrowException_If_There_Is_NoRoles()
        {
            var command = new UnAssignUserRoleCommand()
            {
                UserId = "123465"
            };

            Assert.ThrowsAsync<BusinessException>(async () =>
               await authorizationAppService.UnAssignRole(command, CancellationToken.None));
        }


        [Test]
        public void UnAssignUserRole_ThrowException_If_There_Is_An_Invalid_Roles()
        {
            var command = new UnAssignUserRoleCommand()
            {
                UserId = "123465",
                Roles = new List<RemoveRoleCommand>()
                {
                    new RemoveRoleCommand(){
                    Scope="",
                    RoleName="User"
                    }
                }
            };

           var bex =  Assert.ThrowsAsync<BusinessException>(async () =>
               await authorizationAppService.UnAssignRole(command, CancellationToken.None));

            Assert.IsNotNull(bex);
            Assert.AreEqual(bex.Errors.Count(), 1);
            Assert.AreEqual(bex.Errors.First().Message, "Scope is required.");
        }


        [Test]
        public async Task UnAssignUserRole()
        {
            var userId = Guid.NewGuid();
            var roleUser = "Admin";
            var roleScope = "User";


            var actualUser = new AuthUser()
            {
                Id = userId.ToString(),
                DomainId = Guid.NewGuid().ToString()               
            };

            actualUser.AssignRole(new Role() { Name= roleUser, Scope = roleScope });
            actualUser.AssignRole(new Role() { Name= "Admin", Scope = "Financial" });

            authorizationRepositoryMock.GetUserByExternalId(userId.ToString(), Arg.Any<CancellationToken>()).Returns(actualUser);

          

            var command = new UnAssignUserRoleCommand()
            {
                UserId =userId.ToString(),
                Roles = new List<RemoveRoleCommand>()
                {
                    new RemoveRoleCommand(){
                    Scope=roleScope,
                    RoleName=roleUser
                    }
                }
            };

            await authorizationAppService.UnAssignRole(command, CancellationToken.None);

            await authorizationRepositoryMock.Received(1).Save(Arg.Any<AuthUser>(), Arg.Any<CancellationToken>());

            Assert.IsNotNull(actualUser.Roles);
            Assert.AreEqual(actualUser.Roles.Count,1);
            Assert.AreEqual(actualUser.Roles.First().Scope, "Financial");
        }
    }
}