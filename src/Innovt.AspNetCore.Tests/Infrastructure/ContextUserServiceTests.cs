// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore.Tests

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Innovt.AspNetCore.Infrastructure;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.AspNetCore.Tests.Infrastructure;

[TestFixture]
public class ContextUserServiceTests
{
    private IHttpContextAccessor httpContextAccessorMock;

    [SetUp]
    public void Setup()
    {
        httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
    }

    [TearDown]
    public void TearDown()
    {
        httpContextAccessorMock = null;
    }

    #region Failure Cases

    [Test]
    public void Constructor_Should_ThrowArgumentNullException_When_HttpContextAccessor_Is_Null()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ContextUserService(null!));
    }

    [Test]
    public void GetCurrentUser_Should_ReturnNull_When_HttpContext_Is_Null()
    {
        // Arrange
        httpContextAccessorMock.HttpContext.Returns((HttpContext)null);
        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCurrentUser_Should_ReturnNull_When_User_Is_Null()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.User = null!;
        httpContextAccessorMock.HttpContext.Returns(httpContext);
        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCurrentUser_Should_ReturnNull_When_User_Is_Not_Authenticated()
    {
        // Arrange
        var identity = Substitute.For<ClaimsIdentity>();
        identity.IsAuthenticated.Returns(false);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetCurrentUser_Should_ReturnNull_When_Identity_Is_Null()
    {
        // Arrange
        var principal = new ClaimsPrincipal();

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region Success Cases

    [Test]
    public void GetCurrentUser_Should_Return_ContextUser_With_Basic_Claims()
    {
        // Arrange
        var userId = "user123";
        var email = "user@example.com";
        var name = "John Doe";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Roles, Is.Null);
            Assert.That(result.Claims, Is.Null);
        }
    }

    [Test]
    public void GetCurrentUser_Should_Return_ContextUser_With_UserId_Only_When_Email_And_Name_Are_Missing()
    {
        // Arrange
        var userId = "user123";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Email, Is.Null);
            Assert.That(result.Name, Is.Null);
            Assert.That(result.Roles, Is.Null);
            Assert.That(result.Claims, Is.Null);
        }
    }

    [Test]
    public void GetCurrentUser_Should_Return_ContextUser_With_Empty_UserId_When_NameIdentifier_Is_Missing()
    {
        // Arrange
        var email = "user@example.com";
        var name = "John Doe";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.UserId, Is.EqualTo(string.Empty));
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Name, Is.EqualTo(name));
        }
    }

    [Test]
    public void GetCurrentUser_Should_Return_ContextUser_With_Roles()
    {
        // Arrange
        var userId = "user123";
        var role1 = "Admin";
        var role2 = "User";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role1),
            new Claim(ClaimTypes.Role, role2)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Roles, Is.Not.Null);
        }
        Assert.That(result.Roles.Count, Is.EqualTo(2));
        Assert.That(result.Roles, Does.Contain(role1));
        Assert.That(result.Roles, Does.Contain(role2));
    }

    [Test]
    public void GetCurrentUser_Should_Return_ContextUser_With_Custom_Claims()
    {
        // Arrange
        var userId = "user123";
        var customClaimType1 = "department";
        var customClaimValue1 = "Engineering";
        var customClaimType2 = "level";
        var customClaimValue2 = "Senior";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(customClaimType1, customClaimValue1),
            new Claim(customClaimType2, customClaimValue2)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Claims, Is.Not.Null);
        }
        Assert.That(result.Claims.Count, Is.EqualTo(2));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Claims[customClaimType1], Is.EqualTo(customClaimValue1));
            Assert.That(result.Claims[customClaimType2], Is.EqualTo(customClaimValue2));
        }
    }

    [Test]
    public void GetCurrentUser_Should_Return_ContextUser_With_All_Data_Combined()
    {
        // Arrange
        var userId = "user123";
        var email = "user@example.com";
        var name = "John Doe";
        var role1 = "Admin";
        var role2 = "Manager";
        var customClaimType1 = "department";
        var customClaimValue1 = "Engineering";
        var customClaimType2 = "level";
        var customClaimValue2 = "Senior";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role1),
            new Claim(ClaimTypes.Role, role2),
            new Claim(customClaimType1, customClaimValue1),
            new Claim(customClaimType2, customClaimValue2)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Name, Is.EqualTo(name));

            Assert.That(result.Roles, Is.Not.Null);
        }
        Assert.That(result.Roles.Count, Is.EqualTo(2));
        Assert.That(result.Roles, Does.Contain(role1));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Roles, Does.Contain(role2));

            Assert.That(result.Claims, Is.Not.Null);
        }
        Assert.That(result.Claims.Count, Is.EqualTo(2));
        Assert.That(result.Claims[customClaimType1], Is.EqualTo(customClaimValue1));
        Assert.That(result.Claims[customClaimType2], Is.EqualTo(customClaimValue2));
    }

    [Test]
    public void GetCurrentUser_Should_Exclude_Standard_Claims_From_Custom_Claims_Dictionary()
    {
        // Arrange
        var userId = "user123";
        var email = "user@example.com";
        var name = "John Doe";
        var role = "Admin";
        var customClaim = "custom";
        var customValue = "value";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role),
            new Claim(customClaim, customValue)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.User = principal;
        httpContextAccessorMock.HttpContext.Returns(httpContext);

        var service = new ContextUserService(httpContextAccessorMock);

        // Act
        var result = service.GetCurrentUser();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Claims, Is.Not.Null);
        Assert.That(result.Claims.Count, Is.EqualTo(1));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Claims.ContainsKey(ClaimTypes.NameIdentifier), Is.False);
            Assert.That(result.Claims.ContainsKey(ClaimTypes.Email), Is.False);
            Assert.That(result.Claims.ContainsKey(ClaimTypes.Name), Is.False);
            Assert.That(result.Claims.ContainsKey(ClaimTypes.Role), Is.False);
            Assert.That(result.Claims.ContainsKey(customClaim), Is.True);
            Assert.That(result.Claims[customClaim], Is.EqualTo(customValue));
        }
    }

    #endregion
}