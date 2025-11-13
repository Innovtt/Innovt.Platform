// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Test.Models;
using Innovt.Core.Utilities;
using NUnit.Framework;

namespace Innovt.Core.Test;

[TestFixture]
public class ExtensionsTests
{
    [Test]
    public void CheckRemoveAccents()
    {
        var actual = ("Hoje é o dia mais feliz da minha vida. Espero que isso funcione. " +
                      "Esse código foi baixado da WEB e ainda não tenho como testar sem´aspas '").NormalizeText();

        var expected =
            "Hoje e o dia mais feliz da minha vida Espero que isso funcione Esse codigo foi baixado da WEB e ainda nao tenho como testar sem aspas";


        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void CheckHasSpecialCharacter()
    {
        const string invalidCharacters = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        foreach (var character in invalidCharacters) Assert.That(("name" + character).HasSpecialCharacter(), Is.True);

        Assert.That("name".HasSpecialCharacter(), Is.False);
    }

    [Test]
    public void PagedCollectionMapShouldReturnEmptyListWhenArrayIsNull()
    {
        PagedCollection<Invoice> invoices = null;

        var invoiceDtoPagedCollection = invoices.MapToPagedCollection<Invoice, InvoiceDto>();

        Assert.That(invoiceDtoPagedCollection, Is.Not.Null);
    }


    [Test]
    public void PagedCollectionMap()
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.Now,
            BidId = Guid.NewGuid(),
            ErpId = "ErpId",
            BuyerId = Guid.NewGuid(),
            BuyerName = "michel"
        };

        var invoices = new List<Invoice>
        {
            invoice
        };

        var invoicePagedConnection = new PagedCollection<Invoice>(invoices, 1, 10);

        var invoiceDtoPagedCollection = invoicePagedConnection.MapToPagedCollection<Invoice, InvoiceDto>();


        Assert.That(invoiceDtoPagedCollection, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(invoiceDtoPagedCollection.Page, Is.EqualTo(invoicePagedConnection.Page));
            Assert.That(invoiceDtoPagedCollection.PageSize, Is.EqualTo(invoicePagedConnection.PageSize));
            Assert.That(invoiceDtoPagedCollection.TotalRecords, Is.EqualTo(invoicePagedConnection.TotalRecords));
            Assert.That(invoiceDtoPagedCollection.Items, Has.Exactly(1).Items);
        });

        var invoiceDto = invoiceDtoPagedCollection.Items.SingleOrDefault();
        Assert.That(invoiceDto, Is.Not.Null);
        Assert.That(invoiceDto.Id, Is.EqualTo(invoice.Id));
        Assert.That(invoiceDto.CreatedAt, Is.EqualTo(invoice.CreatedAt));
        Assert.That(invoiceDto.BidId, Is.EqualTo(invoice.BidId));
        Assert.That(invoiceDto.ErpId, Is.EqualTo(invoice.ErpId));
        Assert.That(invoiceDto.BuyerId, Is.EqualTo(invoice.BuyerId));
        Assert.That(invoiceDto.BuyerName, Is.EqualTo(invoice.BuyerName));
    }


    [Test]
    public void ConvertDateTimeBetweenTimeZones()
    {
        var dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0, DateTimeKind.Utc);
        var fromOffSet = new TimeSpan(-3, 0, 0); //Brazil is UTC -3
        var destinationTimeZoneId = new TimeSpan(2, 0, 0); //Spain is UTC +2

        var convertedDateTime = dateTime.ToTimeZone(fromOffSet, destinationTimeZoneId);

        Assert.That(convertedDateTime, Is.EqualTo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            12, 0, 0, DateTimeKind.Utc)));
    }

    [Test]
    public void ConvertDateTimeOffSetBetweenTimeZones()
    {
        var dateTime = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0,
            new TimeSpan(-3, 0, 0));
        var destinationTimeZone = new TimeSpan(2, 0, 0); //Spain is UTC +2

        var convertedDateTime = dateTime.ToTimeZone(destinationTimeZone);

        Assert.That(convertedDateTime, Is.EqualTo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            12, 0, 0, DateTimeKind.Utc)));
    }

    [TestCase("", "")]
    [TestCase(null, "")]
    [TestCase(" ", "")]
    [TestCase("   ", "")]
    [TestCase("---", "")]
    [TestCase("Hello World!", "hello-world")]
    [TestCase("Hello & World", "hello-world")]
    [TestCase("Hello (World)", "hello-world")]
    [TestCase("Hello @ World", "hello-world")]
    [TestCase("Hello #World", "hello-world")]
    [TestCase("Hello's World", "hellos-world")]
    [TestCase("áéíóú", "aeiou")]
    [TestCase("çñ", "cn")]
    [TestCase("äëïöü", "aeiou")]
    [TestCase("àèìòù", "aeiou")]
    [TestCase("Café", "cafe")]
    [TestCase("Product 2024", "product-2024")]
    [TestCase("Version 1.0.0", "version-1-0-0")]
    [TestCase("Item #123", "item-123")]
    [TestCase("hello--world", "hello-world")]
    [TestCase("hello---world", "hello-world")]
    [TestCase("hello - world", "hello-world")]
    [TestCase("-hello-world-", "hello-world")]
    [TestCase("Hello World (Special Edition) 2024 ®", "hello-world-special-edition-2024")]
    [TestCase("L'été est très beau!", "lete-est-tres-beau")]
    [TestCase("∑∆ Special Characters ♥†", "special-characters")]
    [TestCase("User's Guide & Tutorial", "users-guide-tutorial")]
    public void CreateSlug(string value, string expected)
    {
        var actual = value.CreateSlug();

        Assert.That(actual, Is.EqualTo(expected));
    }


    [Test]
    public void LongInput_ShouldBeTruncated()
    {
        var longInput = new string('a', 100) + "-test";
        var result = longInput.CreateSlug(75);
        Assert.That(result, Has.Length.LessThanOrEqualTo(75));
    }


    [Test]
    public void ToUri_ReturnsNull_If_Str_IsNull()
    {
        string? uri = null;

        var result = uri.ToUri();
        Assert.That(result, Is.Null);
    }

    [Test]
    public void ToUri_ReturnsNull_If_Str_IsEmpty()
    {
        var uri = string.Empty;

        var result = uri.ToUri();
        Assert.That(result, Is.Null);
    }

    [Test]
    public void ToUri_ReturnsN_If_Uri_Is_NotValid()
    {
        var uri = "invalid-url";

        var result = uri.ToUri();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsAbsoluteUri, Is.False);
        Assert.That(result.ToString(), Is.EqualTo("invalid-url"));
    }

    [Test]
    public void ToUri()
    {
        var uri = "http://example.com/path?query=123";

        var result = uri.ToUri();

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsAbsoluteUri, Is.True);
            Assert.That(result.ToString(), Is.EqualTo(uri));
        }
    }


    [TestCase(null)]
    [TestCase("")]
    public void ToDeterministicGuidThrowsExceptionWhenEmailIsNUll(string email)
    {
        if (email == null)
            Assert.Throws<ArgumentNullException>(() => email.ToDeterministicGuid());
        else
            Assert.Throws<ArgumentException>(() => email.ToDeterministicGuid());
    }

    [TestCase("michelmob@gmail.com", "f9369c7e-3298-d066-7200-9ce835dad432")]
    [TestCase("MICHELMOB@GMAIL.COM", "f9369c7e-3298-d066-7200-9ce835dad432")]
    public void ToDeterministicGuid(string email, string expectedGuid)
    {
        var result = email.ToDeterministicGuid();

        Assert.That(result, Is.Not.Empty);
        Assert.That(result.ToString(), Is.EqualTo(expectedGuid));
    }
    
    
    [TestCase("1981/04/02", "1981/03/30")]
    [TestCase("2025/11/13", "2025/11/10")]
    public void GetMondayOfAWeek(string date, string expected)
    {
        var providedDay = DateTime.Parse(date, new DateTimeFormatInfo());
        var expectedDate = DateTime.Parse(expected,new DateTimeFormatInfo());
        
        var actualDate = providedDay.GetMondayOfWeek();
        Assert.That(actualDate, Is.EqualTo(expectedDate));
    }
    
    
    [TestCase("1981/04/02", "1981/04/03")]
    [TestCase("2025/11/13", "2025/11/14")]
    public void GetFridayOfAWeek(string date, string expected)
    {
        var providedDay = DateTime.Parse(date, new DateTimeFormatInfo());
        var expectedDate = DateTime.Parse(expected,new DateTimeFormatInfo());
        
        var actualDate = providedDay.GetFridayOfWeek();
        Assert.That(actualDate, Is.EqualTo(expectedDate));
    }
}