using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries
{
    public interface IQueryHandler<in TFilter, out TResult> where TFilter : IFilter where TResult : class
    {
        TResult Handle(TFilter filter);
    }
}