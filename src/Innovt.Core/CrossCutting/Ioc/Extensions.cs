// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Linq;
using System.Reflection;
using Innovt.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc;

public static class Extensions
{
    public static void AddModule(this IServiceCollection services, Assembly assembly)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        var modulesTypes = assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(IOCModule))).ToList();

        foreach (var moduleType in modulesTypes)
        {
            var module = (IOCModule)Activator.CreateInstance(moduleType.UnderlyingSystemType, services);

            if (module == null)
                throw new ConfigurationException("Innovt - IOC Module not found.");
        }
    }

    public static void AddModule(this IServiceCollection services, IOCModule module)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        if (module is null) throw new ArgumentNullException(nameof(module));

        var servicesModule = module.GetServices();


        if (servicesModule != null)
            foreach (var service in servicesModule)
                if (!services.Contains(service))
                    services.Add(service);
    }
}