using System;
using System.Threading;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Domain;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Contrib.Authorization.Platform.Tests;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
        authorizationRepositoryMock = Substitute.For<IAuthorizationRepository>();

        authorizationAppService = new AuthorizationAppService(authorizationRepositoryMock);
    }

    private IAuthorizationRepository authorizationRepositoryMock;
    private IAuthorizationAppService authorizationAppService;

    [Test]
    public void AddUser_ShouldThrowException_When_Command_Is_Null()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => authorizationAppService.AddUser(null, CancellationToken.None));
    }
}