// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

namespace Innovt.Domain.Core.Model;

public class SimpleVo<T> : ValueObject<T> where T : struct
{
    public SimpleVo()
    {
    }

    public SimpleVo(T id, string description)
    {
        Id = id;
        Description = description;
    }

    public string Description { get; set; }
}