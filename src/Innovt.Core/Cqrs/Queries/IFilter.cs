// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries;

public interface IFilter : IValidatableObject
{
}

public interface IPagedFilter : IFilter
{
    int Page { get; set; }

    int PageSize { get; set; }
}