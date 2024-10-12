// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Linq;
using System.Reflection;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc;

/// <summary>
///     Provides a locator for resolving services and managing dependency injection containers.
/// </summary>
/// <remarks>
///     This static class acts as a locator for resolving services and managing dependency injection containers.
///     It provides methods for initializing the container, resolving services, adding modules, releasing resources,
///     creating service scopes, and checking container configuration.
/// </remarks>
public static class IocLocator
{
    private static IContainer container;


    /// <summary>
    ///     Initializes the locator with the specified main dependency injection container.
    /// </summary>
    /// <param name="mainContainer">The main <see cref="IContainer" /> to be used for service resolution.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mainContainer" /> is null.</exception>
    public static void Initialize(IContainer mainContainer)
    {
        // Implementation of the Initialize method.
        container = mainContainer ?? throw new ArgumentNullException(nameof(mainContainer));
    }

    private static void ThrowExceptionIfContainerIsNotInitialized()
    {
        // Implementation of the ThrowExceptionIfContainerIsNotInitialized method.
        if (container == null)
            throw new CriticalException("IOC Container not initialized");
    }


    /// <summary>
    ///     Resolves a service of the specified <paramref name="type" />.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> of the service to resolve.</param>
    /// <returns>The resolved service object.</returns>
    public static object Resolve(Type type)
    {
        // Implementation of the Resolve method.
        ThrowExceptionIfContainerIsNotInitialized();

        return container.Resolve(type);
    }


    /// <summary>
    ///     Resolves a service of type <typeparamref name="TService" /> with the specified <paramref name="type" />.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="type">The <see cref="Type" /> of the service implementation to resolve.</param>
    /// <returns>The resolved service object.</returns>
    public static TService Resolve<TService>(Type type)
    {
        // Implementation of the Resolve method.
        ThrowExceptionIfContainerIsNotInitialized();

        return container.Resolve<TService>(type);
    }

    /// <summary>
    ///     Resolves a named instance of a service of type <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="intanceKey">The key identifying the named instance to resolve.</param>
    /// <returns>The resolved service object.</returns>
    public static TService Resolve<TService>(string intanceKey)
    {
        // Implementation of the Resolve method.
        ThrowExceptionIfContainerIsNotInitialized();

        return container.Resolve<TService>(intanceKey);
    }

    /// <summary>
    ///     Resolves a service of type <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <returns>The resolved service object.</returns>
    public static TService Resolve<TService>()
    {
        // Implementation of the Resolve method.
        ThrowExceptionIfContainerIsNotInitialized();

        return container.Resolve<TService>();
    }

    /// <summary>
    ///     Adds an <see cref="IocModule" /> to the container.
    /// </summary>
    /// <param name="module">The <see cref="IocModule" /> to be added to the container.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="module" /> is null.</exception>
    public static void AddModule(IocModule module)
    {
        // Implementation of the AddModule method.
        Check.NotNull(module);

        ThrowExceptionIfContainerIsNotInitialized();

        container.AddModule(module);
    }

    /// <summary>
    ///     Releases the specified object and frees associated resources.
    /// </summary>
    /// <param name="obj">The object to release.</param>
    public static void Release(object obj)
    {
        // Implementation of the Release method
        ThrowExceptionIfContainerIsNotInitialized();

        container.Release(obj);
    }

    /// <summary>
    ///     Creates and returns a new service scope.
    /// </summary>
    /// <returns>An <see cref="IServiceScope" /> representing the new service scope.</returns>
    public static IServiceScope CreateScope()
    {
        // Implementation of the CreateScope method.
        ThrowExceptionIfContainerIsNotInitialized();

        return container.CreateScope();
    }

    /// <summary>
    ///     Adds modules from the specified <paramref name="assembly" /> to the container.
    /// </summary>
    /// <param name="assembly">The assembly containing modules to be added.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assembly" /> is null.</exception>
    public static void AddModuleFromAssembly(Assembly assembly)
    {
        // Implementation of the AddModuleFromAssembly method.
        Check.NotNull(assembly);

        ThrowExceptionIfContainerIsNotInitialized();

        var moduleType = typeof(IocModule);
        var modules = assembly.GetTypes().Where(t => t.GetTypeInfo().IsSubclassOf(moduleType)).ToList();

        foreach (var module in modules)
        {
            var moduleInstance = (IocModule)Activator.CreateInstance(module);

            container.AddModule(moduleInstance);
        }
    }


    /// <summary>
    ///     Checks the configuration of the container.
    /// </summary>
    public static void CheckConfiguration()
    {
        // Implementation of the CheckConfiguration method.
        ThrowExceptionIfContainerIsNotInitialized();
        container.CheckConfiguration();
    }
}