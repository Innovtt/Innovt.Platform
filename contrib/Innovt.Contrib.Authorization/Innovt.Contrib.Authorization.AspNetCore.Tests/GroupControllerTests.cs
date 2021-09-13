using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.AspNetCore.Tests
{
    [TestFixture()]    
    public class GroupControllerTests
    {
        private IAuthorizationAppService authorizationAppServiceMock;
        private GroupController groupController;

        public GroupControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            authorizationAppServiceMock = Substitute.For<IAuthorizationAppService>();

            groupController = new GroupController(authorizationAppServiceMock);
        }

        [Test]
        public void GetShouldThrowExceptionWhenFilterIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async ()=> await groupController.Get(null));
        }

        [Test]
        public void AddShouldThrowExceptionWhenFilterIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await groupController.Add(null));
        }

        [Test]
        public void RemoveShouldThrowExceptionWhenFilterIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await groupController.Add(null));
        }

        [Test]
        public async Task Get()
        {
            var groupsExpected = new List<GroupDto>();

            authorizationAppServiceMock.FindGroupBy(Arg.Any<GroupFilter>(), Arg.Any<CancellationToken>()).Returns(groupsExpected);

            var actual = await groupController.Get(new GroupFilter());

            Assert.IsNotNull(actual);

            await authorizationAppServiceMock.Received(1).FindGroupBy(Arg.Any<GroupFilter>(), Arg.Any<CancellationToken>());
        }
    }
}