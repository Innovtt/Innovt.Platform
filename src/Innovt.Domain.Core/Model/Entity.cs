// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using Innovt.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Model;

/// <summary>
///     Represents an abstract base class for entities in the domain model.
/// </summary>
public abstract class Entity<T> where T : struct
{
    private readonly List<DomainEvent> domainEvents;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Entity" /> class.
    /// </summary>
    protected Entity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        domainEvents = new List<DomainEvent>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Entity" /> class with a specific identifier.
    /// </summary>
    /// <param name="id">The identifier for the entity.</param>
    protected Entity(T id) : this()
    {
        Id = id;
    }

    /// <summary>
    ///     Gets or sets the unique identifier for the entity.
    /// </summary>
    public T Id { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Checks if the entity is new (i.e., not persisted in the database yet).
    /// </summary>
    /// <returns><c>true</c> if the entity is new; otherwise, <c>false</c>.</returns>
    public bool IsNew()
    {
        return Id.Equals(default(T));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is Entity<T> entity &&
               EqualityComparer<T>.Default.Equals(Id, entity.Id);
    }

    /// <summary>
    ///     Adds a domain event to the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    public Entity<T> AddDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

        domainEvent.PublishedAt = null;
        domainEvent.CreatedAt = DateTime.UtcNow;
        domainEvents.Add(domainEvent);
        return this;
    }

    /// <summary>
    ///     Gets the list of unprocessed domain events associated with the entity.
    /// </summary>
    /// <returns>A read-only list of unprocessed domain events.</returns>
    public IReadOnlyList<DomainEvent> GetDomainEvents()
    {
        return domainEvents.Where(d => d.PublishedAt is null).ToList().AsReadOnly();
    }
}

/// <summary>
///     Represents an abstract base class for entities in the domain model with a specific type for the identifier.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class Entity : Entity<int>
{
}