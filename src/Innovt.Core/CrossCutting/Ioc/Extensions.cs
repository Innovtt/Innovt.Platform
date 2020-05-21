using System;
using System.Linq;
using System.Reflection;
using Innovt.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc
{
    public static class Extensions
    {

        /// <summary>
        /// helper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void AddModule(this IServiceCollection services,Assembly assembly)
        {
            var modulesTypes = assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(IOCModule))).ToList();

            foreach (var moduleType in modulesTypes)
            {
                var module = (IOCModule)Activator.CreateInstance(moduleType.UnderlyingSystemType, services);
                
                if(module==null)
                    throw new ConfigurationException("Innovt - IOC Module not found.");
            }
            
        }
    }
}