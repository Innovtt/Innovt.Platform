
namespace Innovt.Domain.Core.Model
{
    public abstract class ValueObject
    {
        public virtual int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (obj is null) return false;

            return (obj as ValueObject)?.Id == this.Id;
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