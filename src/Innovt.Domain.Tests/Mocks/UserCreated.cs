using Innovt.Domain.Core.Events;

namespace Innovt.Domain.Tests.Mocks
{
    public class UserCreated : DomainEvent
    {
        public UserCreated(string name, string version, string partition) : base(name, version, partition)
        {
        }

        public UserCreated() : base(nameof(UserCreated), nameof(UserCreated))
        {
        }
    }
}