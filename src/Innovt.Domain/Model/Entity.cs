using System;

namespace Innovt.Domain.Model
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        protected Entity()
        {

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
    }

    public abstract class Entity<T>: Entity where T : struct
    {
        public new T Id { get; set; }
        
    }
}