// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Microsoft.Extensions.DependencyInjection;
using System;

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