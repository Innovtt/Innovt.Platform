
using System;
using System.Collections.Generic;
using Innovt.Domain.Core.Events;

namespace Innovt.Domain.Core.Model
{
    public interface IAggregateRoot<T>
    {
        public T Id { get; set; }
    }

    public interface IAggregateRoot
    {
        public int Id { get; set; }
    }
}