// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore.Tests

using System.Security.Claims;
using Innovt.AspNetCore.Filters;
using NUnit.Framework;

namespace Innovt.AspNetCore.Tests;

[TestFixture]
public class InjectClaimParameterFilterAttributeTests
{
    [Test]
    public void ConstructorShould_SetDefaultValues()
    {
        var filter = new InjectClaimParameterFilterAttribute();
        
        Assert.That(filter.ActionParameters, Is.Not.Null);
        Assert.That(filter.DefaultAuthorizationProperty, Is.Not.Null);
        Assert.That(filter.ClaimTypeCheck, Is.Not.Null);
        
        Assert.That(filter.ActionParameters, Is.Not.Null);
        Assert.That(filter.DefaultAuthorizationProperty, Is.EqualTo("ExternalId"));
        Assert.That(filter.ClaimTypeCheck, Is.EqualTo(ClaimTypes.NameIdentifier));
    }
    
    [Test]
    public void ConstructorShould_SetCustomValues()
    {
        var filter = new InjectClaimParameterFilterAttribute("UserId");
        
        Assert.That(filter.ActionParameters, Is.Not.Null);
        Assert.That(filter.DefaultAuthorizationProperty, Is.Not.Null);
        Assert.That(filter.ClaimTypeCheck, Is.Not.Null);
        
        Assert.That(filter.ActionParameters.Length,Is.EqualTo(2));
        Assert.That(filter.ActionParameters[0],Is.EqualTo("filter"));
        Assert.That(filter.ActionParameters[1],Is.EqualTo("command"));
        Assert.That(filter.DefaultAuthorizationProperty, Is.EqualTo("UserId"));
        Assert.That(filter.ClaimTypeCheck, Is.EqualTo(ClaimTypes.NameIdentifier));
    }
    
    
    [Test]
    public void ConstructorShould_SetCustomActionValues()
    {
        var filter = new InjectClaimParameterFilterAttribute("UserId",ClaimTypes.Actor,"filter2");
        
        Assert.That(filter.ActionParameters, Is.Not.Null);
        Assert.That(filter.DefaultAuthorizationProperty, Is.Not.Null);
        Assert.That(filter.ClaimTypeCheck, Is.Not.Null);
        
        Assert.That(filter.ActionParameters.Length,Is.EqualTo(1));
        Assert.That(filter.ActionParameters[0],Is.EqualTo("filter2"));
        Assert.That(filter.DefaultAuthorizationProperty, Is.EqualTo("UserId"));
        Assert.That(filter.ClaimTypeCheck, Is.EqualTo(ClaimTypes.Actor));
    }



}