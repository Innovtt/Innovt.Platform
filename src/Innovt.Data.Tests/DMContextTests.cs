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
        var instance = DmContext.Instance();
        Assert.That(instance, Is.Not.Null);
    }


    [Test]
    public void AttachThrowExceptionIfObjectIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => DmContext.Instance().Attach<UserDataModel>(null));
    }


    [Test]
    public void DeAttachThrowExceptionIfObjectIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => DmContext.Instance().DeAttach<UserDataModel>(null));
    }

    [Test]
    public void FindThrowExceptionIfObjectIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => DmContext.Instance().Find<UserDataModel>(null));
    }

    [Test]
    public void CheckHashCode()
    {
        var userDataModel = new UserDataModel
        {
            Id = 10,
            Name = "Michel",
            Address = "Rua a",
            LastName = "Borges"
        };

        DmContext.Instance().Attach(userDataModel);

        var userDataModel2 = new UserDataModel
        {
            Id = 10,
            Name = "Michel",
            Address = "Rua a",
            LastName = "Borges"
        };

        DmContext.Instance().Attach(userDataModel2);

        DmContext.Instance().DeAttach(userDataModel);
        DmContext.Instance().DeAttach(userDataModel2);
    }

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

        DmContext.Instance().Attach(userDataModel);

        var user = DmContext.Instance().Find(userDataModel);

        Assert.That(user, Is.Not.Null);

        Assert.That(userDataModel.Name, Is.EqualTo(user.Name));
        Assert.That(userDataModel.Id, Is.EqualTo(user.Id));
        Assert.That(userDataModel.LastName, Is.EqualTo(user.LastName));
        Assert.That(userDataModel.Address, Is.EqualTo(user.Address));
    }
}