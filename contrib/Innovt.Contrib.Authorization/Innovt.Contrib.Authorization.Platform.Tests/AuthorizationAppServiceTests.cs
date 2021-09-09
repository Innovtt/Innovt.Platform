using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        public void AddRole_ThrowException_If_CommandIsNUll()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await authorizationAppService.AddRole(null, CancellationToken.None));
        }
        
        [TestCase(null, "Allow Admin users",null)]
        public void AddRole_ThrowException_If_Command_IsNotValid(string name, string description,Guid[] permissions)
        {
            var command = new AddRoleCommand(name,description, permissions);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AddRole(command, CancellationToken.None));
        }

        [Test]
        public async Task AddRole_Throw_Exception_If_Role_Already_Exist()
        {
            var expectedRole = new Role() {Name = "Admin", Description = "Allow Admin users"};

            var command = new AddRoleCommand("Admin", "Allow Admin users", null);

            authorizationRepositoryMock.GetRoleByName(command.Name, Arg.Any<CancellationToken>())
                .Returns(expectedRole);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AddRole(command, CancellationToken.None), "Role already exist.");
      
            await authorizationRepositoryMock.Received(1).GetRoleByName(command.Name, Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task AddRole()
        {
            var command = new AddRoleCommand("Admin", "Allow Admin users", null);

            authorizationRepositoryMock.GetRoleByName(command.Name, Arg.Any<CancellationToken>())
                .Returns(default(Role));

            var roleId = await authorizationAppService.AddRole(command, CancellationToken.None);

            Assert.IsNotNull(roleId);
            Assert.IsFalse(roleId == Guid.Empty);

            await authorizationRepositoryMock.Received(1).GetRoleByName(command.Name, Arg.Any<CancellationToken>());
            await authorizationRepositoryMock.Received(1).AddRole(Arg.Any<Role>(), Arg.Any<CancellationToken>());
        }

        //permission
        [Test]
        public void AddPermission_ThrowException_If_CommandIsNUll()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await authorizationAppService.AddPermission(null, CancellationToken.None));
        }

        [TestCase(null, "", null)]
        [TestCase("AllowGet", "", null)]
        [TestCase("AllowGet", "user", null)]
        public void AddPermission_ThrowException_If_Command_IsNotValid(string name, string domain, string resource)
        {
            var command = new AddPermissionCommand(name,domain,resource);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AddPermission(command, CancellationToken.None));
        }

        [Test]
        public async Task AddPermission_Throw_Exception_If_Permission_Already_Exist()
        {
            var expectedPermission = new Permission() { Name = "AllowGet", Domain = "user", Resource = "/user" };

            var command = new AddPermissionCommand(expectedPermission.Name, expectedPermission.Domain, expectedPermission.Resource);
            
            authorizationRepositoryMock.GetPermissionsBy(command.Scope,command.Resource,null, Arg.Any<CancellationToken>())
                .Returns(new List<Permission>(){expectedPermission});

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AddPermission(command, CancellationToken.None), "Permission already exist.");

            await authorizationRepositoryMock.Received(1).GetPermissionsBy(command.Scope, command.Resource, null,
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task AddPermission()
        {
            var command = new AddPermissionCommand("Admin", "Allow Admin users", "/users");
            
            authorizationRepositoryMock.GetPermissionsBy(command.Scope, command.Resource, null, Arg.Any<CancellationToken>())
                .Returns(default(List<Permission>));

            var permissionId = await authorizationAppService.AddPermission(command, CancellationToken.None);

            Assert.IsNotNull(permissionId);
            Assert.IsFalse(permissionId == Guid.Empty);

            await authorizationRepositoryMock.Received(1).GetPermissionsBy(command.Scope, command.Resource, null,
                Arg.Any<CancellationToken>());

            await authorizationRepositoryMock.Received(1).AddPermission(Arg.Any<Permission>(),Arg.Any<CancellationToken>());
        }
        
        //Group
        [Test]
        public void AddGroup_ThrowException_If_CommandIsNUll()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await authorizationAppService.AddGroup(null, CancellationToken.None));
        }
        
        [TestCase(null, "user", "user group")]
        [TestCase("Admin", "", "user group")]
        [TestCase("Admin", "user", null)]
        public void AddGroup_ThrowException_If_Command_IsNotValid(string name, string scope, string description)
        {
            var command = new AddGroupCommand(name,scope,description);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AddGroup(command, CancellationToken.None));
        }

        [Test]
        public async Task AddGroup_Throw_Exception_If_Group_Already_Exist()
        {
            var group = new Group() { Name = "AllowGet", Domain = "user", Description = "Group of users"};

            var command = new AddGroupCommand(group.Name, group.Domain, group.Description);

            authorizationRepositoryMock.GetGroupBy(command.Name, command.Scope, Arg.Any<CancellationToken>())
                .Returns(group);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await authorizationAppService.AddGroup(command, CancellationToken.None), "Group already exist.");

            await authorizationRepositoryMock.Received(1).GetGroupBy(command.Name,command.Scope,
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task AddGroup()
        {
            var command = new AddGroupCommand("Admin", "User","Group admin");
            
            authorizationRepositoryMock.GetGroupBy(command.Name, command.Scope, Arg.Any<CancellationToken>())
                .Returns(default(Group));

            var groupId = await authorizationAppService.AddGroup(command, CancellationToken.None);

            Assert.IsNotNull(groupId);
            Assert.IsFalse(groupId == Guid.Empty);

            await authorizationRepositoryMock.Received(1)
                .GetGroupBy(command.Name, command.Scope, Arg.Any<CancellationToken>());

            await authorizationRepositoryMock.Received(1).AddGroup(Arg.Any<Group>(), Arg.Any<CancellationToken>());
        }
    }
}