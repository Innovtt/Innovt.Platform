// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System.Collections.Generic;
using System.Text.Json;
using Innovt.Core.Exceptions;
using NUnit.Framework;

namespace Innovt.Core.Test.Exceptions;

[TestFixture]
public class BusinessExceptionTests
{
    [Test]
    public void CheckMessage()
    {
        var bex = new BusinessException("E-mail is not valid.");
        
        Assert.IsNotNull(bex);

        Assert.That("E-mail is not valid.", Is.EqualTo(bex.Message));
    }


    [Test]
    public void CheckCode()
    {
        var bex = new BusinessException("01","E-mail is not valid.");

        Assert.IsNotNull(bex);

        Assert.That("E-mail is not valid.", Is.EqualTo(bex.Message));
        Assert.That("01", Is.EqualTo(bex.Code));
    }

    [Test]
    public void MessageIsEmptyWhenErrorListIsEmpty()
    {
        var bex = new BusinessException(new List<ErrorMessage>());

        Assert.IsNotNull(bex);
        Assert.IsNotNull(bex.Errors);
        Assert.That(bex.Message, Is.EqualTo("One or more validation errors occurred."));
    }


    [Test]
    public void CheckValidationPattern()
    {
        var errors = new List<ErrorMessage>()
        {
            new ErrorMessage("e-mail is required", "Email", "01"),
            new ErrorMessage("invalid e-mail", "Email", "02"),
            new ErrorMessage("name is required", "Name", "02")
        };

        var bex = new BusinessException(errors);

        Assert.IsNotNull(bex);
        Assert.IsNotNull(bex.Errors);
        Assert.That(errors.Count, Is.EqualTo(3));
        Assert.That(JsonSerializer.Serialize(bex.Detail), Is.EqualTo("[{\"Property\":\"Email\",\"Errors\":[{\"Code\":\"01\",\"Message\":\"e-mail is required\"},{\"Code\":\"02\",\"Message\":\"invalid e-mail\"}]},{\"Property\":\"Name\",\"Errors\":[{\"Code\":\"02\",\"Message\":\"name is required\"}]}]"));
    }

}