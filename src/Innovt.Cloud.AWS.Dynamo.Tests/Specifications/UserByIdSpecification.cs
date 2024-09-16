using System;
using System.Linq.Expressions;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using Innovt.Domain.Core.Specification;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Specifications;

public class UserByIdSpecification: ISpecification<User>
{
    public UserByIdSpecification(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public Expression<Func<User, bool>> SatisfiedBy()
    {
       var spec = new DirectSpecification<User>(o => o.Id == Id.ToString() && o.IsActive);

       return spec.SatisfiedBy();

    }
}