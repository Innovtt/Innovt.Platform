// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using System.Collections.Generic;
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
        Assert.That(invoiceDtoPagedCollection.Page, Is.EqualTo(invoicePagedConnection.Page));
        Assert.That(invoiceDtoPagedCollection.PageSize, Is.EqualTo(invoicePagedConnection.PageSize));
        Assert.That(invoiceDtoPagedCollection.TotalRecords, Is.EqualTo(invoicePagedConnection.TotalRecords));
        Assert.That(invoiceDtoPagedCollection.Items, Has.Exactly(1).Items);

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
}