// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Tests

using System;
using Innovt.Data.DataModels;
using Innovt.Data.Tests.DataModel;
using NUnit.Framework;

namespace Innovt.Data.Tests;

[TestFixture]
public class DMContextTests
{
    [Test]
    public void InstanceCantBeNullWhenUsingSingleton()
    {
        var instance = DMContext.Instance();
        Assert.That(instance, Is.Not.Null);
    }


    [Test]
    public void AttachThrowExceptionIfObjectIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => DMContext.Instance().Attach<UserDataModel>(null));
    }


    [Test]
    public void DeAttachThrowExceptionIfObjectIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => DMContext.Instance().DeAttach<UserDataModel>(null));
    }

    [Test]
    public void FindThrowExceptionIfObjectIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => DMContext.Instance().Find<UserDataModel>(null));
    }

    [Test]
    public void CheckHashCode()
    {
        var userDataModel = new UserDataModel()
        {
            Id = 10,
            Name = "Michel",
            Address = "Rua a",
            LastName = "Borges"
        };

        DMContext.Instance().Attach<UserDataModel>(userDataModel);

        var userDataModel2 = new UserDataModel()
        {
            Id = 10,
            Name = "Michel",
            Address = "Rua a",
            LastName = "Borges"
        };

        DMContext.Instance().Attach<UserDataModel>(userDataModel2);

        DMContext.Instance().DeAttach<UserDataModel>(userDataModel);
        DMContext.Instance().DeAttach<UserDataModel>(userDataModel2);
    }

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

        DMContext.Instance().Attach<UserDataModel>(userDataModel);

        var user = DMContext.Instance().Find<UserDataModel>(userDataModel);

        Assert.That(user, Is.Not.Null);

        Assert.That(userDataModel.Name, Is.EqualTo(user.Name));
        Assert.That(userDataModel.Id, Is.EqualTo(user.Id));
        Assert.That(userDataModel.LastName, Is.EqualTo(user.LastName));
        Assert.That(userDataModel.Address, Is.EqualTo(user.Address));
    }
}