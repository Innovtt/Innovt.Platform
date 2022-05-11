// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Events;
using System;
using System.Collections.Generic;

namespace Innovt.Domain.Core.Model;

public abstract class Entity
{
    private List<DomainEvent> domainEvents;

    protected Entity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    protected Entity(int id)
    {
        Id = id;
    }

    public virtual int Id { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public bool IsNew()
    {
        return Id == 0;
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public override bool Equals(object obj)
    {
        var anotherEntity = obj as Entity;

        return anotherEntity?.Id == Id;
    }

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

        domainEvents ??= new List<DomainEvent>();

        domainEvents.Add(domainEvent);
    }

    public IReadOnlyList<DomainEvent> GetDomainEvents()
    {
        return domainEvents?.AsReadOnly();
    }
}

public abstract class Entity<T> : Entity where T : struct
{
    public new T Id { get; set; }
}