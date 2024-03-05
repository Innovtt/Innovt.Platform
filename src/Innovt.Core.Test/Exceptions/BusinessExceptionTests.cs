// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System.Collections.Generic;
using System.Text.Json;
using Innovt.Core.Exceptions;
using NUnit.Framework;

namespace Innovt.Core.Test.Exceptions;

/// <summary>
///     Unit tests for the <see cref="BusinessException" /> class.
/// </summary>
[TestFixture]
public class BusinessExceptionTests
{
    /// <summary>
    ///     Verifies that the exception message is set correctly.
    /// </summary>
    [Test]
    public void CheckMessage()
    {
        var bex = new BusinessException("E-mail is not valid.");

        Assert.That(bex, Is.Not.Null);

        Assert.That(bex.Message, Is.EqualTo("E-mail is not valid."));
    }

    /// <summary>
    ///     Verifies that the exception code and message are set correctly.
    /// </summary>
    [Test]
    public void CheckCode()
    {
        var bex = new BusinessException("01", "E-mail is not valid.");

        Assert.That(bex, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(bex.Message, Is.EqualTo("E-mail is not valid."));
            Assert.That(bex.Code, Is.EqualTo("01"));
        });
    }

    /// <summary>
    ///     Verifies that the exception message is set to a default value when the error list is empty.
    /// </summary>
    [Test]
    public void MessageIsEmptyWhenErrorListIsEmpty()
    {
        var bex = new BusinessException(new List<ErrorMessage>());

        Assert.That(bex, Is.Not.Null);
        Assert.That(bex.Errors, Is.Not.Null);
        Assert.That(bex.Message, Is.EqualTo("One or more validation errors occurred."));
    }

    /// <summary>
    ///     Verifies that the exception detail is null when the error list is empty.
    /// </summary>
    [Test]
    public void DetailShouldBeNullWhenErrorListIsEmpty()
    {
        var bex = new BusinessException(new List<ErrorMessage>());

        Assert.That(bex, Is.Not.Null);
        Assert.That(bex.Errors, Is.Not.Null);
        Assert.That(bex.Detail, Is.Null);
    }

    /// <summary>
    ///     Verifies that the exception is created correctly with a list of error messages.
    /// </summary>
    [Test]
    public void CheckValidationPattern()
    {
        var errors = new List<ErrorMessage>
        {
            new("e-mail is required", "Email", "01"),
            new("invalid e-mail", "Email", "02"),
            new("name is required", "Name", "02")
        };

        var bex = new BusinessException(errors);

        Assert.That(bex, Is.Not.Null);
        Assert.That(bex.Errors, Is.Not.Null);
        Assert.That(errors.Count, Is.EqualTo(3));
        Assert.That(JsonSerializer.Serialize(bex.Detail),
            Is.EqualTo(
                "[{\"Property\":\"Email\",\"Errors\":[{\"Code\":\"01\",\"Message\":\"e-mail is required\"},{\"Code\":\"02\",\"Message\":\"invalid e-mail\"}]},{\"Property\":\"Name\",\"Errors\":[{\"Code\":\"02\",\"Message\":\"name is required\"}]}]"));
    }
}