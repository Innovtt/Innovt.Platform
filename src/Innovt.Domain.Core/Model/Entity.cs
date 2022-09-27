// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Domain.Core.Events;

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

        domainEvent.PublishedAt = null;

        domainEvents.Add(domainEvent);
    }

    public IReadOnlyList<DomainEvent> GetDomainEvents()
    {
        return domainEvents?.Where(d => d.PublishedAt is null).ToList().AsReadOnly();
    }
}

public abstract class Entity<T> : Entity where T : struct
{
    public new T Id { get; set; }
}