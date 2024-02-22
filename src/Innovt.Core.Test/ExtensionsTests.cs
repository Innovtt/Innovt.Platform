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
}