// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.HealthChecks;

public interface IServiceHealthCheck
{
    string Name { get; set; }
    bool Check();
}