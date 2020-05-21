using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries
{
    public interface IFilter:IValidatableObject
    {

    }

    public interface IPagedFilter : IFilter
    {
        int Page { get; set; }

        int PageSize { get; set; }
    }
}
