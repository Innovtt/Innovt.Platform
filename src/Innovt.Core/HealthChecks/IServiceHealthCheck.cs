// COMPANY: INNOVT TECNOLOGIA
// PROJECT: Innovt.Core
// DATE: 11-15-2019
// AUTHOR: michel

namespace Innovt.Core.HealthChecks
{
    public interface IServiceHealthCheck
    {
        string Name { get; set; }
        bool Check();
    }
}