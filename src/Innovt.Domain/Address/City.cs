using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Address
{
    public class City : ValueObject
    {
        public string Name { get; set; }

        public int StateId { get; set; }

        public virtual State State { get; set; }
    }
}
