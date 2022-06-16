using Innovt.Domain.Tests.Mocks;
using NUnit.Framework;

namespace Innovt.Domain.Tests
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void CheckDomainEvents()
        {
            var user = new UserEntity();

            var events = user.GetDomainEvents();

            Assert.IsNull(events);

            user.AddDomainEvent(new UserCreated());

            events = user.GetDomainEvents();

            Assert.IsNotNull(events);

            Assert.AreEqual(1,events.Count);
        }
    }
}