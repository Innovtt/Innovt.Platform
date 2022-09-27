// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc;

public interface IContainer : IDisposable
{
    void AddModule(IOCModule iocModule);

    object Resolve(Type type);

    TService Resolve<TService>();

    TService Resolve<TService>(Type type);

    TService Resolve<TService>(string instanceKey);

    void Release(object obj);

    IServiceScope CreateScope();

    void CheckConfiguration();
}