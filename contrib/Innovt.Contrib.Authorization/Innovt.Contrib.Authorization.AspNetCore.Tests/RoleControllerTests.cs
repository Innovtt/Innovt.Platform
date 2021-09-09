using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.AspNetCore;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Infrastructure;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Antecipa.Onboarding.Api.Tests
{
    [TestFixture()]    
    public class RoleControllerTests
    {
        private IAuthorizationAppService authorizationAppServiceMock;
        private RoleController roleController;
        public RoleControllerTests()
        {
        }

        [SetUp]
        public void Setup()
        {
            //authorizationAppServiceMock = Substitute.For<IAuthorizationAppService>();
            IAwsConfiguration configuration = new DefaultAWSConfiguration();
            var logger = new Innovt.CrossCutting.Log.Serilog.Logger();

            authorizationAppServiceMock = new AuthorizationAppService(new AuthorizationRepository(logger,configuration));

            roleController = new RoleController(authorizationAppServiceMock);
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
        public async Task AddRole()
        {
            var command = new AddRoleCommand()
            {
                Name= "Admin",
                Description= "Admin Role",
                Scope = "user"
            };

            await roleController.Add(command);
        }
    }
}