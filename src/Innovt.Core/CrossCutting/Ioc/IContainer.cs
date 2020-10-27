using System;

namespace Innovt.Core.CrossCutting.Ioc
{
    public interface IContainer: IDisposable
    {
        void AddModule(IOCModule module);
 
        object Resolve(Type type);
        TService Resolve<TService>();
        TService Resolve<TService>(Type type);
        TService Resolve<TService>(string instanceKey);
        void CheckConfiguration();
    }
}
