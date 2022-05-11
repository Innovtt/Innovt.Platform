// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Core.HealthChecks;

public interface IServiceHealthCheck
{
    string Name { get; set; }
    bool Check();
}