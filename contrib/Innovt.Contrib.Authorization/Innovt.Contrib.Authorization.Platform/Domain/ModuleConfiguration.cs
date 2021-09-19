// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-08-07

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public class ModuleConfiguration
    {
        public ModuleConfiguration(string moduleName)
        {
            Name = moduleName;
        }

        public string Name { get; set; }
    }
}