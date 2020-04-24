using System.Collections.Generic;

namespace Innovt.Domain.Model
{
    public abstract class ValueObject
    {
        public virtual int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (this.GetType() != obj.GetType()) return false;


            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public abstract class ValueObject<T>: ValueObject where T:struct
    {
        public new T Id { get; set; }
    }
}