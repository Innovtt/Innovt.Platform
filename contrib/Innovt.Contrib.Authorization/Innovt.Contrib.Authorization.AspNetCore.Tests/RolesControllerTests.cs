// Company: Antecipa
// Project: Innovt.Contrib.Authorization.AspNetCore.Tests
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-07

using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Infrastructure;
using Innovt.CrossCutting.Log.Serilog;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.AspNetCore.Tests
{
    [TestFixture]
    public class RolesControllerTests
    {
        [SetUp]
        public void Setup()
        {
            //authorizationAppServiceMock = Substitute.For<IAuthorizationAppService>();
            IAwsConfiguration configuration = new DefaultAWSConfiguration();
            var logger = new Logger();

            authorizationAppServiceMock =
                new AuthorizationAppService(new AuthorizationRepository(logger, configuration));

            roleController = new RolesController(authorizationAppServiceMock);
        }

        private IAuthorizationAppService authorizationAppServiceMock;
        private RolesController roleController;

        //[Test]
        //public void GetShouldThrowExceptionWhenFilterIsNull()
        //{
        //    Assert.ThrowsAsync<ArgumentNullException>(async ()=> await permissionController..Get(null));
        //}

        //[Test]
        //public void AddShouldThrowExceptionWhenFilterIsNull()
        //{
        //    Assert.ThrowsAsync<ArgumentNullException>(async () => await groupController.Add(null));
        //}

        //[Test]
        //public void RemoveShouldThrowExceptionWhenFilterIsNull()
        //{
        //    Assert.ThrowsAsync<ArgumentNullException>(async () => await groupController.Add(null));
        //}

        [Test]
        public async Task AddRole()
        {
            var command = new AddRoleCommand
            {
                Name = "Admin",
                Description = "Admin Role",
                Scope = "user"
            };

            await roleController.Add(command);
        }
    }
}