using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.AspNetCore.Tests
{
    [TestFixture()]    
    public class UserControllerTests
    {
        private IAuthorizationAppService authorizationAppServiceMock;
        private UserController userController;

        public UserControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            authorizationAppServiceMock = Substitute.For<IAuthorizationAppService>();

            userController = new UserController(authorizationAppServiceMock);
        }

        [Test]
        public void AddShouldThrowExceptionWhenFilterIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await userController.AddUser(null, CancellationToken.None));
        }
        
        [Test]
        public async Task Add()
        {
            var userCommand = new AddUserCommand();

            authorizationAppServiceMock.AddUser(userCommand, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            var result =  await userController.AddUser(userCommand);

            Assert.IsNotNull(result);
            
            await authorizationAppServiceMock.Received(1).AddUser(userCommand, Arg.Any<CancellationToken>());
        }
    }
}