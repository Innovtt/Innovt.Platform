using Innovt.Data.Tests.DataModel;
using NUnit.Framework;

namespace Innovt.Data.Tests
{
    [TestFixture]
    public class DataModelTests
    {
        [Test]
        public void Attach()
        {
            var userDataModel = new UserDataModel()
            {
                Id = 10,
                Name = "Michel",
                Address = "Rua a",
                LastName = "Borges"
            };

            userDataModel.EnableTrackingChanges = true;

            Assert.IsFalse(userDataModel.HasChanges);

            userDataModel.Name = "Marcio";

            Assert.IsTrue(userDataModel.HasChanges);
        }
    }
}