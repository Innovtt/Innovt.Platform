using System;
using System.Collections.Generic;
using Innovt.Domain.Core.Events;

namespace Innovt.Domain.Core.Model
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        protected Entity()
        {
            CreatedAt  = DateTimeOffset.UtcNow;
        }

        protected Entity(int id)
        {
            Id = id;
        }

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
        
        private List<DomainEvent> domainEvents;
      
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

    public abstract class Entity<T>: Entity where T : struct
    {
        public new T Id { get; set; }
    }
}