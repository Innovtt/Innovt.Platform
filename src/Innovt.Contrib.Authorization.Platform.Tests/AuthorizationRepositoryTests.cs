using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.IOC;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.Platform.Tests
{
    public class Tests
    {

        private readonly IAuthorizationRepository authorizationRepository;

        public Tests()
        {   
            var container = new Container();
            
            var services =  new ServiceCollection();

            services.AddTransient<IAwsConfiguration>(a => new DefaultAWSConfiguration("antecipa-dev"));

            container.AddModule(new AuthorizationModule(services));
            
            container.CheckConfiguration();

            authorizationRepository = container.Resolve<Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationRepository>();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Ignore("Testing antecipa env")]
        public async Task GetUserByExternalId()
        {
            var user= await authorizationRepository.GetUserByExternalId("michel@antecipa.com", CancellationToken.None);
            
            Assert.IsNotNull(user);
        }
    }
}