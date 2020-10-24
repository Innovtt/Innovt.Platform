
namespace Innovt.Domain.Core.Model
{
    public interface IAggregateRoot<T>
    {
        public T Id { get; set; }
    }

    public interface IAggregateRoot
    {
       // public T Id { get; set; }
    }
}