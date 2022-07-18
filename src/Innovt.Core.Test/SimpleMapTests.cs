using Innovt.Core.Test.Models;
using Innovt.Core.Utilities;
using NUnit.Framework;
using System;

namespace Innovt.Core.Test;

internal class SomeDomain
{
    public string Name { get; set; }

    public string Description { get; set; }
}

internal class SomeDomain2 : SomeDomain
{
    public int Age { get; set; }
}

internal class SomeDomain3 : SomeDomain
{
    public string Age { get; set; }
}

internal class SomeDto
{
    public string Name { get; set; }

    public string Description { get; set; }
}

internal class SomeDto2 : SomeDto
{
    public int Age { get; set; }
}

internal class SomeDto3 : SomeDto
{
    public int Age { get; set; }
}

[TestFixture]
public class SimpleMapTests
{
    [Test]
    public void MapReturnsNullWhenInputIsNull()
    {
        var output = SimpleMapper.Map<SomeDto, SomeDomain>(null!);

        Assert.IsNull(output);
    }


    [Test]
    public void MapReturnsAvailableProperties()
    {
        var b = new SomeDto { Name = "michel", Description = "something" };

        var output = SimpleMapper.Map<SomeDto, SomeDomain>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name, output.Name);
        Assert.AreEqual(b.Description, output.Description);
    }

    [Test]
    public void MapReturnsPropertiesThatExistsOnlyInInput()
    {
        var b = new SomeDto2 { Name = "michel", Description = "something", Age = 2 };

        var output = SimpleMapper.Map<SomeDto2, SomeDomain>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name, output.Name);
        Assert.AreEqual(b.Description, output.Description);
    }

    [Test]
    public void MapKeepPropertyDefaultIfThePropertyWasNotFoundInTheInput()
    {
        var b = new SomeDto { Name = "michel", Description = "something" };

        var output = SimpleMapper.Map<SomeDto, SomeDomain2>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name, output.Name);
        Assert.AreEqual(b.Description, output.Description);
        Assert.AreEqual(0, output.Age);
    }


    [Test]
    public void MapPropertyWithSameType()
    {
        var b = new SomeDto3 { Name = "michel", Description = "something" };

        var output = SimpleMapper.Map<SomeDto3, SomeDomain3>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name, output.Name);
        Assert.AreEqual(b.Description, output.Description);
        Assert.IsNull(output.Age);
    }


    [Test]
    public void MapWithInstances()
    {
        var a = new SomeDto { Name = "michel", Description = "something" };
        var b = new SomeDomain2();

        SimpleMapper.Map(a, b);

        Assert.IsNotNull(b);
        Assert.AreEqual(a.Name, b.Name);
        Assert.AreEqual(a.Description, b.Description);
    }

    [Test]
    public void MapWithComplexInstancesValidatePropertyAmbiguity()
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.Now,
            BidId = Guid.NewGuid(),
            ErpId = "ErpId",
            BuyerId = Guid.NewGuid(),
            BuyerName = "michel",
            BuyerErpId = "BuyerErpId",
            BuyerDocument = "BuyerDocument",
            BuyerGroupId = Guid.NewGuid(),
            BuyerGroupName = "SupplierErpId",
            SupplierId = Guid.NewGuid(),
            SupplierName = "SupplierName",
            SupplierErpId = "SupplierErpId",
            SupplierDocument = "supplierDocument",
            CurrencyId = 1,
            Currency = "R$",
            Description = "fake invoice",
            Interest = 1,
            Fine = 10,
            Tax = 10,
            FiscalDocumentNumbers = "1,2,3",
            InstallmentNumber = "2",
            PaymentTypeId = 1,
            PaymentType = "credit",
            PaymentOrderStatusId = 2,
            PaymentOrderStatus = "PaymentOrderStatus",
            BankSlipBarCode = "BankSlipBarCode",
            Metadata = "null",
            DaysToDue = 5,
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now,
            Discount = 5,
            Value = 200,
            NetValue = 200,
            PaymentDate = DateTime.Now,
            PaymentValue = 100,
            UpdatedAt = DateTime.Now
        };

        var invoiceDto = new InvoiceDto();

        SimpleMapper.Map(invoice, invoiceDto);

        Assert.IsNotNull(invoiceDto);
        Assert.AreEqual(invoice.Description, invoiceDto.Description);
        Assert.AreEqual(invoice.CreatedAt, invoiceDto.CreatedAt);
        Assert.AreEqual(invoice.BuyerId, invoiceDto.BuyerId);
        Assert.AreEqual(invoice.BuyerName, invoiceDto.BuyerName);
        Assert.AreEqual(invoice.BuyerErpId, invoiceDto.BuyerErpId);
        Assert.AreEqual(invoice.BuyerDocument, invoiceDto.BuyerDocument);
        Assert.AreEqual(invoice.BuyerGroupId, invoiceDto.BuyerGroupId);
        Assert.AreEqual(invoice.SupplierId, invoiceDto.SupplierId);
        Assert.AreEqual(invoice.SupplierName, invoiceDto.SupplierName);
        Assert.AreEqual(invoice.SupplierErpId, invoiceDto.SupplierErpId);
        Assert.AreEqual(invoice.SupplierDocument, invoiceDto.SupplierDocument);
        Assert.AreEqual(invoice.CurrencyId, invoiceDto.CurrencyId);
        Assert.AreEqual(invoice.Currency, invoiceDto.Currency);
        Assert.AreEqual(invoice.Interest, invoiceDto.Interest);
        Assert.AreEqual(invoice.Fine, invoiceDto.Fine);
        Assert.AreEqual(invoice.Tax, invoiceDto.Tax);
        Assert.AreEqual(invoice.FiscalDocumentNumbers, invoiceDto.FiscalDocumentNumbers);
        Assert.AreEqual(invoice.InstallmentNumber, invoiceDto.InstallmentNumber);
        Assert.AreEqual(invoice.PaymentTypeId, invoiceDto.PaymentTypeId);
        Assert.AreEqual(invoice.PaymentType, invoiceDto.PaymentType);
        Assert.AreEqual(invoice.PaymentOrderStatusId, invoiceDto.PaymentOrderStatusId);
        Assert.AreEqual(invoice.PaymentOrderStatus, invoiceDto.PaymentOrderStatus);
        Assert.AreEqual(invoice.BankSlipBarCode, invoiceDto.BankSlipBarCode);
        Assert.AreEqual(invoice.Metadata, invoiceDto.Metadata);
        Assert.AreEqual(invoice.DaysToDue, invoiceDto.DaysToDue);
        Assert.AreEqual(invoice.IssueDate, invoiceDto.IssueDate);
        Assert.AreEqual(invoice.DueDate, invoiceDto.DueDate);
        Assert.AreEqual(invoice.Discount, invoiceDto.Discount);
        Assert.AreEqual(invoice.Value, invoiceDto.Value);
        Assert.AreEqual(invoice.NetValue, invoiceDto.NetValue);
        Assert.AreEqual(invoice.PaymentDate, invoiceDto.PaymentDate);
        Assert.AreEqual(invoice.PaymentValue, invoiceDto.PaymentValue);
        Assert.AreEqual(invoice.UpdatedAt, invoiceDto.UpdatedAt);
    }
}