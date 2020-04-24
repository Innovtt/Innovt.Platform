using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc
{
    public static class Extensions
    {
        public static void AddModule(this IServiceCollection services,Assembly assembly)
        {
            var modulesTypes = assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(IOCModule))).ToList();

            foreach (var moduleType in modulesTypes)
            {
                var module = (IOCModule)Activator.CreateInstance(moduleType.UnderlyingSystemType, services);
                
                if(module==null)
                    throw  new Exception("Innovt - Module not found.");
            }
            
        }
    }
}