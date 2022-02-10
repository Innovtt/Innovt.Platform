// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Innovt.Core.CrossCutting.Ioc
{
    public static class IOCLocator
    {
        private static IContainer container;

        public static void Initialize(IContainer mainContainer)
        {
            container = mainContainer ?? throw new ArgumentNullException(nameof(mainContainer));
        }

        private static void ThrowExceptionIfContainerIsNotInitialized()
        {
            if (container == null)
                throw new CriticalException("IOC Container not initialized");
        }

        public static object Resolve(Type type)
        {
            ThrowExceptionIfContainerIsNotInitialized();

            return container.Resolve(type);
        }

        public static TService Resolve<TService>(Type type)
        {
            ThrowExceptionIfContainerIsNotInitialized();

            return container.Resolve<TService>(type);
        }

        public static TService Resolve<TService>(string intanceKey)
        {
            ThrowExceptionIfContainerIsNotInitialized();

            return container.Resolve<TService>(intanceKey);
        }


        public static TService Resolve<TService>()
        {
            ThrowExceptionIfContainerIsNotInitialized();

            return container.Resolve<TService>();
        }

        public static void AddModule(IOCModule module)
        {
            if (module == null) throw new ArgumentNullException(nameof(module));

            ThrowExceptionIfContainerIsNotInitialized();

            container.AddModule(module);
        }


        public static void Release(object obj)
        {
            ThrowExceptionIfContainerIsNotInitialized();

            container.Release(obj);
        }

        public static IServiceScope CreateScope()
        {
            ThrowExceptionIfContainerIsNotInitialized();

            return container.CreateScope();
        }

        public static void AddModuleFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            ThrowExceptionIfContainerIsNotInitialized();

            var moduleType = typeof(IOCModule);
            var modules = assembly.GetTypes().Where(t => t.GetTypeInfo().IsSubclassOf(moduleType)).ToList();

            foreach (var module in modules)
            {
                var moduleInstance = (IOCModule)Activator.CreateInstance(module);

                container.AddModule(moduleInstance);
            }
        }

        public static void CheckConfiguration()
        {
            ThrowExceptionIfContainerIsNotInitialized();
            container.CheckConfiguration();
        }
    }
}