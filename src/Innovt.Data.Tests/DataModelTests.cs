// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Tests

using Innovt.Data.Tests.DataModel;
using NUnit.Framework;

namespace Innovt.Data.Tests;

[TestFixture]
public class DataModelTests
{
    [Test]
    public void Attach()
    {
        var userDataModel = new UserDataModel
        {
            Id = 10,
            Name = "Michel",
            Address = "Rua a",
            LastName = "Borges"
        };

        userDataModel.EnableTrackingChanges = true;

        Assert.That(userDataModel.HasChanges, Is.False);

        userDataModel.Name = "Marcio";

        Assert.That(userDataModel.HasChanges, Is.True);
    }
}