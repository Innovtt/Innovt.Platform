// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Linq;
using System.Reflection;
using Innovt.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc;

/// <summary>
///     Provides extension methods for working with <see cref="IServiceCollection" /> and dependency injection.
/// </summary>
/// <remarks>
///     This static class contains extension methods that enhance the functionality of the
///     <see cref="IServiceCollection" />
///     interface, enabling easier registration of services and modules.
/// </remarks>
public static class Extensions
{
    /// <summary>
    ///     Adds services defined in modules from the specified assembly to the <paramref name="services" /> collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which services should be added.</param>
    /// <param name="assembly">The assembly containing modules to be added.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services" /> or <paramref name="assembly" /> is null.</exception>
    public static void AddModule(this IServiceCollection services, Assembly assembly)
    {
        // Implementation of the AddModule method.
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        var modulesTypes = assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(IocModule))).ToList();

        foreach (var moduleType in modulesTypes)
        {
            var module = (IocModule)Activator.CreateInstance(moduleType.UnderlyingSystemType, services);

            if (module == null)
                throw new ConfigurationException("Innovt - IOC Module not found.");
        }
    }

    /// <summary>
    ///     Adds services from the specified <paramref name="module" /> to the <paramref name="services" /> collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to which services should be added.</param>
    /// <param name="module">The <see cref="IocModule" /> containing services to be added.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services" /> or <paramref name="module" /> is null.</exception>
    public static void AddModule(this IServiceCollection services, IocModule module)
    {
        // Implementation of the AddModule method.
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(module);

        var servicesModule = module.GetServices();

        if (servicesModule == null) return;

        foreach (var service in servicesModule)
            if (!services.Contains(service))
                services.Add(service);
    }
}