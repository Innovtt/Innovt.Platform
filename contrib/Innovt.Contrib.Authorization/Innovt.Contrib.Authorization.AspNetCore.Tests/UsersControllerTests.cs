// Company: Antecipa
// Project: Innovt.Contrib.Authorization.AspNetCore.Tests
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-12

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.AspNetCore.Tests
{
    [TestFixture]
    public class UsersControllerTests
    {
        [SetUp]
        public void Setup()
        {
            authorizationAppServiceMock = Substitute.For<IAuthorizationAppService>();

            userController = new UsersController(authorizationAppServiceMock);
        }

        private IAuthorizationAppService authorizationAppServiceMock;
        private UsersController userController;

        [Test]
        public void AddShouldThrowExceptionWhenFilterIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await userController.AddUser(null, CancellationToken.None));
        }

        [Test]
        public async Task Add()
        {
            var userCommand = new AddUserCommand();

            authorizationAppServiceMock.AddUser(userCommand, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            var result = await userController.AddUser(userCommand);

            Assert.IsNotNull(result);

            await authorizationAppServiceMock.Received(1).AddUser(userCommand, Arg.Any<CancellationToken>());
        }
    }
}