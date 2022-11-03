using Innovt.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApiTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IAuthorizationRepository authorizationRepository;


        public WeatherForecastController(IAuthorizationRepository authorizationRepository)
        {
            this.authorizationRepository = authorizationRepository ?? throw new ArgumentNullException(nameof(authorizationRepository));
        }


        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            try
            {
                var res = await authorizationRepository.GetUserByExternalId("michel@antecipa.com",CancellationToken.None);
            
            return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}