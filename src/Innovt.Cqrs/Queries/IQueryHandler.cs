using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries
{
    public interface IQueryHandler<T,R> where T:IFilter where R:class
    {
        R Handle(T filter);                          
    }
}
