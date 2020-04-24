using System;

namespace Innovt.Core.CrossCutting.Ioc
{
    public interface IContainer
    {
        void AddModule(IOCModule module);

        void Register<T>(T type);
        void Register<T>(T type,string instanceKey);
        
        void Release(object inst);

        object Resolve(Type type);
        TService Resolve<TService>();
        TService Resolve<TService>(Type type);
        TService Resolve<TService>(string intanceKey);
        TService Resolve<TService>(Type type,string intanceKey);

        void CheckConfiguration();
    }
}
