using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Contrib.Authorization.Platform.Infrastructure;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.AspNetCore.Tests
{
    [TestFixture()]    
    public class PermissionControllerTests
    {
        private IAuthorizationAppService authorizationAppServiceMock;
        private PermissionController permissionController;

        public PermissionControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            //authorizationAppServiceMock = Substitute.For<IAuthorizationAppService>();
            IAwsConfiguration configuration = new DefaultAWSConfiguration();
            var logger = new Innovt.CrossCutting.Log.Serilog.Logger();

            authorizationAppServiceMock = new AuthorizationAppService(new AuthorizationRepository(logger,configuration));

            permissionController = new PermissionController(authorizationAppServiceMock);
        }

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
        public async Task GetPermissions()
        {
            var filter = new PermissionFilter()
            {       
                Scope = "user",
            };
            await permissionController.Get(filter);
        }

        [Test]
        public async Task AddPermission()
        {
            var command = new AddPermissionCommand()
            {
                Scope = "user",
                Name = "Get User",
                Resource = "/user/get"
            };
            await permissionController.Add(command);
        }
    }
}