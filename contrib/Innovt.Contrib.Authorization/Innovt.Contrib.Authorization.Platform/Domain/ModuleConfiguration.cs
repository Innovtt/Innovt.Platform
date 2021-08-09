using System;
using System.Collections.Generic;
using System.Text;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public class ModuleConfiguration
    {
        public string Name { get; set; }
        public ModuleConfiguration(string moduleName)
        {
            this.Name =  moduleName;
        }
        
    }
}
