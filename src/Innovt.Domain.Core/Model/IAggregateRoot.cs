// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

namespace Innovt.Domain.Core.Model;

public interface IAggregateRoot<T>
{
    public T Id { get; set; }
}

public interface IAggregateRoot
{
    public int Id { get; set; }
}