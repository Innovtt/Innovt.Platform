using System;
using System.Threading;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Domain;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.Platform.Tests
{
    [TestFixture]
    public class Tests
    {
        private IAuthorizationRepository authorizationRepositoryMock;
        private IAuthorizationAppService authorizationAppService;



        [SetUp]
        public void Setup()
        {
            authorizationRepositoryMock = NSubstitute.Substitute.For<IAuthorizationRepository>();

            authorizationAppService = new AuthorizationAppService(authorizationRepositoryMock);
        }

        [Test]
        public void AddUser_ShouldThrowException_When_Command_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => authorizationAppService.AddUser(null, CancellationToken.None));
        }

    }
}