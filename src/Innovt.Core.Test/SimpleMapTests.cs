// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Test.Models;
using Innovt.Core.Utilities;
using Innovt.Core.Utilities.Mapper;
using NUnit.Framework;

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
        Assert.That(output, Is.Null);
    }


    [Test]
    public void MapReturnsAvailableProperties()
    {
        var b = new SomeDto { Name = "michel", Description = "something" };

        var output = SimpleMapper.Map<SomeDto, SomeDomain>(b);

        Assert.That(output, Is.Not.Null);

        Assert.That(b.Name, Is.EqualTo(output.Name));

        Assert.That(b.Name, Is.EqualTo(output.Name));
        Assert.That(b.Description, Is.EqualTo(output.Description));
    }

    [Test]
    public void MapReturnsPropertiesThatExistsOnlyInInput()
    {
        var b = new SomeDto2 { Name = "michel", Description = "something", Age = 2 };

        var output = SimpleMapper.Map<SomeDto2, SomeDomain>(b);

        Assert.That(output, Is.Not.Null);
        Assert.That(b.Name, Is.EqualTo(output.Name));
        Assert.That(b.Description, Is.EqualTo(output.Description));
    }

    [Test]
    public void MapKeepPropertyDefaultIfThePropertyWasNotFoundInTheInput()
    {
        var b = new SomeDto { Name = "michel", Description = "something" };

        var output = SimpleMapper.Map<SomeDto, SomeDomain2>(b);

        Assert.That(output, Is.Not.Null);
        Assert.That(b.Name, Is.EqualTo(output.Name));
        Assert.That(b.Description, Is.EqualTo(output.Description));
        Assert.That(0, Is.EqualTo(output.Age));
    }


    [Test]
    public void MapPropertyWithSameType()
    {
        var b = new SomeDto3 { Name = "michel", Description = "something" };

        var output = SimpleMapper.Map<SomeDto3, SomeDomain3>(b);

        Assert.That(output, Is.Not.Null);
        Assert.That(b.Name, Is.EqualTo(output.Name));
        Assert.That(b.Description, Is.EqualTo(output.Description));
        Assert.That(output.Age, Is.Null);
    }


    [Test]
    public void MapWithInstances()
    {
        var a = new SomeDto { Name = "michel", Description = "something" };
        var b = new SomeDomain2();

        SimpleMapper.Map(a, b);

        Assert.That(b, Is.Not.Null);
        Assert.That(a.Name, Is.EqualTo(b.Name));
        Assert.That(a.Description, Is.EqualTo(b.Description));
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

        Assert.That(invoiceDto, Is.Not.Null);

        Assert.That(invoice.Description, Is.EqualTo(invoiceDto.Description));
        Assert.That(invoice.CreatedAt, Is.EqualTo(invoiceDto.CreatedAt));
        Assert.That(invoice.BuyerId, Is.EqualTo(invoiceDto.BuyerId));
        Assert.That(invoice.BuyerName, Is.EqualTo(invoiceDto.BuyerName));
        Assert.That(invoice.BuyerErpId, Is.EqualTo(invoiceDto.BuyerErpId));
        Assert.That(invoice.BuyerDocument, Is.EqualTo(invoiceDto.BuyerDocument));
        Assert.That(invoice.BuyerGroupId, Is.EqualTo(invoiceDto.BuyerGroupId));
        Assert.That(invoice.SupplierId, Is.EqualTo(invoiceDto.SupplierId));
        Assert.That(invoice.SupplierName, Is.EqualTo(invoiceDto.SupplierName));
        Assert.That(invoice.SupplierErpId, Is.EqualTo(invoiceDto.SupplierErpId));
        Assert.That(invoice.SupplierDocument, Is.EqualTo(invoiceDto.SupplierDocument));
        Assert.That(invoice.CurrencyId, Is.EqualTo(invoiceDto.CurrencyId));
        Assert.That(invoice.Currency, Is.EqualTo(invoiceDto.Currency));
        Assert.That(invoice.Interest, Is.EqualTo(invoiceDto.Interest));
        Assert.That(invoice.Fine, Is.EqualTo(invoiceDto.Fine));
        Assert.That(invoice.Tax, Is.EqualTo(invoiceDto.Tax));
        Assert.That(invoice.FiscalDocumentNumbers, Is.EqualTo(invoiceDto.FiscalDocumentNumbers));
        Assert.That(invoice.InstallmentNumber, Is.EqualTo(invoiceDto.InstallmentNumber));
        Assert.That(invoice.PaymentTypeId, Is.EqualTo(invoiceDto.PaymentTypeId));
        Assert.That(invoice.PaymentType, Is.EqualTo(invoiceDto.PaymentType));
        Assert.That(invoice.PaymentOrderStatusId, Is.EqualTo(invoiceDto.PaymentOrderStatusId));
        Assert.That(invoice.PaymentOrderStatus, Is.EqualTo(invoiceDto.PaymentOrderStatus));
        Assert.That(invoice.BankSlipBarCode, Is.EqualTo(invoiceDto.BankSlipBarCode));
        Assert.That(invoice.Metadata, Is.EqualTo(invoiceDto.Metadata));
        Assert.That(invoice.DaysToDue, Is.EqualTo(invoiceDto.DaysToDue));
        Assert.That(invoice.IssueDate, Is.EqualTo(invoiceDto.IssueDate));
        Assert.That(invoice.DueDate, Is.EqualTo(invoiceDto.DueDate));
        Assert.That(invoice.Discount, Is.EqualTo(invoiceDto.Discount));
        Assert.That(invoice.Value, Is.EqualTo(invoiceDto.Value));
        Assert.That(invoice.NetValue, Is.EqualTo(invoiceDto.NetValue));
        Assert.That(invoice.PaymentDate, Is.EqualTo(invoiceDto.PaymentDate));
        Assert.That(invoice.PaymentValue, Is.EqualTo(invoiceDto.PaymentValue));
        Assert.That(invoice.UpdatedAt, Is.EqualTo(invoiceDto.UpdatedAt));
    }
    
    
     [Test]
    public void MapToExtension()
    {
        //maps one o
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
        
        var invoiceDto = invoice.MapTo<InvoiceDto>();
        
        Assert.That(invoiceDto, Is.Not.Null);

        Assert.That(invoice.Description, Is.EqualTo(invoiceDto.Description));
        Assert.That(invoice.CreatedAt, Is.EqualTo(invoiceDto.CreatedAt));
        Assert.That(invoice.BuyerId, Is.EqualTo(invoiceDto.BuyerId));
        Assert.That(invoice.BuyerName, Is.EqualTo(invoiceDto.BuyerName));
        Assert.That(invoice.BuyerErpId, Is.EqualTo(invoiceDto.BuyerErpId));
        Assert.That(invoice.BuyerDocument, Is.EqualTo(invoiceDto.BuyerDocument));
        Assert.That(invoice.BuyerGroupId, Is.EqualTo(invoiceDto.BuyerGroupId));
        Assert.That(invoice.SupplierId, Is.EqualTo(invoiceDto.SupplierId));
        Assert.That(invoice.SupplierName, Is.EqualTo(invoiceDto.SupplierName));
        Assert.That(invoice.SupplierErpId, Is.EqualTo(invoiceDto.SupplierErpId));
        Assert.That(invoice.SupplierDocument, Is.EqualTo(invoiceDto.SupplierDocument));
        Assert.That(invoice.CurrencyId, Is.EqualTo(invoiceDto.CurrencyId));
        Assert.That(invoice.Currency, Is.EqualTo(invoiceDto.Currency));
        Assert.That(invoice.Interest, Is.EqualTo(invoiceDto.Interest));
        Assert.That(invoice.Fine, Is.EqualTo(invoiceDto.Fine));
        Assert.That(invoice.Tax, Is.EqualTo(invoiceDto.Tax));
        Assert.That(invoice.FiscalDocumentNumbers, Is.EqualTo(invoiceDto.FiscalDocumentNumbers));
        Assert.That(invoice.InstallmentNumber, Is.EqualTo(invoiceDto.InstallmentNumber));
        Assert.That(invoice.PaymentTypeId, Is.EqualTo(invoiceDto.PaymentTypeId));
        Assert.That(invoice.PaymentType, Is.EqualTo(invoiceDto.PaymentType));
        Assert.That(invoice.PaymentOrderStatusId, Is.EqualTo(invoiceDto.PaymentOrderStatusId));
        Assert.That(invoice.PaymentOrderStatus, Is.EqualTo(invoiceDto.PaymentOrderStatus));
        Assert.That(invoice.BankSlipBarCode, Is.EqualTo(invoiceDto.BankSlipBarCode));
        Assert.That(invoice.Metadata, Is.EqualTo(invoiceDto.Metadata));
        Assert.That(invoice.DaysToDue, Is.EqualTo(invoiceDto.DaysToDue));
        Assert.That(invoice.IssueDate, Is.EqualTo(invoiceDto.IssueDate));
        Assert.That(invoice.DueDate, Is.EqualTo(invoiceDto.DueDate));
        Assert.That(invoice.Discount, Is.EqualTo(invoiceDto.Discount));
        Assert.That(invoice.Value, Is.EqualTo(invoiceDto.Value));
        Assert.That(invoice.NetValue, Is.EqualTo(invoiceDto.NetValue));
        Assert.That(invoice.PaymentDate, Is.EqualTo(invoiceDto.PaymentDate));
        Assert.That(invoice.PaymentValue, Is.EqualTo(invoiceDto.PaymentValue));
        Assert.That(invoice.UpdatedAt, Is.EqualTo(invoiceDto.UpdatedAt));
    }


    [Test]
    public void MapToListWhenSourceListIsNullOrEmptyReturnsEmpty()
    {
        var invoices = new List<Invoice>();
        
        var invoicesDto = invoices.MapToList<InvoiceDto>();
        
        Assert.That(invoicesDto, Is.Not.Null);
        Assert.That(invoicesDto, Is.Empty);
        
        
        invoices = null;
        
        // ReSharper disable once ExpressionIsAlwaysNull
        invoicesDto = invoices.MapToList<InvoiceDto>();
        
        Assert.That(invoicesDto, Is.Not.Null);
        Assert.That(invoicesDto, Is.Empty);
    }

    [Test]
    public void MapToList()
    {
        //maps one o
        var invoices = new List<Invoice>
        {
            new Invoice
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
            }
        };

        var invoicesDto = invoices.MapToList<InvoiceDto>();
        
        Assert.That(invoicesDto, Is.Not.Null);
        Assert.That(invoicesDto.SingleOrDefault(), Is.Not.Null);
        
        var invoice = invoices.SingleOrDefault();
        var invoiceDto = invoicesDto.SingleOrDefault();
            
        Assert.That(invoice, Is.Not.Null);
        Assert.That(invoiceDto, Is.Not.Null);
        Assert.That(invoice.Description, Is.EqualTo(invoiceDto.Description));
        Assert.That(invoice.CreatedAt, Is.EqualTo(invoiceDto.CreatedAt));
        Assert.That(invoice.BuyerId, Is.EqualTo(invoiceDto.BuyerId));
        Assert.That(invoice.BuyerName, Is.EqualTo(invoiceDto.BuyerName));
        Assert.That(invoice.BuyerErpId, Is.EqualTo(invoiceDto.BuyerErpId));
        Assert.That(invoice.BuyerDocument, Is.EqualTo(invoiceDto.BuyerDocument));
        Assert.That(invoice.BuyerGroupId, Is.EqualTo(invoiceDto.BuyerGroupId));
        Assert.That(invoice.SupplierId, Is.EqualTo(invoiceDto.SupplierId));
        Assert.That(invoice.SupplierName, Is.EqualTo(invoiceDto.SupplierName));
        Assert.That(invoice.SupplierErpId, Is.EqualTo(invoiceDto.SupplierErpId));
        Assert.That(invoice.SupplierDocument, Is.EqualTo(invoiceDto.SupplierDocument));
        Assert.That(invoice.CurrencyId, Is.EqualTo(invoiceDto.CurrencyId));
        Assert.That(invoice.Currency, Is.EqualTo(invoiceDto.Currency));
        Assert.That(invoice.Interest, Is.EqualTo(invoiceDto.Interest));
        Assert.That(invoice.Fine, Is.EqualTo(invoiceDto.Fine));
        Assert.That(invoice.Tax, Is.EqualTo(invoiceDto.Tax));
        Assert.That(invoice.FiscalDocumentNumbers, Is.EqualTo(invoiceDto.FiscalDocumentNumbers));
        Assert.That(invoice.InstallmentNumber, Is.EqualTo(invoiceDto.InstallmentNumber));
        Assert.That(invoice.PaymentTypeId, Is.EqualTo(invoiceDto.PaymentTypeId));
        Assert.That(invoice.PaymentType, Is.EqualTo(invoiceDto.PaymentType));
        Assert.That(invoice.PaymentOrderStatusId, Is.EqualTo(invoiceDto.PaymentOrderStatusId));
        Assert.That(invoice.PaymentOrderStatus, Is.EqualTo(invoiceDto.PaymentOrderStatus));
        Assert.That(invoice.BankSlipBarCode, Is.EqualTo(invoiceDto.BankSlipBarCode));
        Assert.That(invoice.Metadata, Is.EqualTo(invoiceDto.Metadata));
        Assert.That(invoice.DaysToDue, Is.EqualTo(invoiceDto.DaysToDue));
        Assert.That(invoice.IssueDate, Is.EqualTo(invoiceDto.IssueDate));
        Assert.That(invoice.DueDate, Is.EqualTo(invoiceDto.DueDate));
        Assert.That(invoice.Discount, Is.EqualTo(invoiceDto.Discount));
        Assert.That(invoice.Value, Is.EqualTo(invoiceDto.Value));
        Assert.That(invoice.NetValue, Is.EqualTo(invoiceDto.NetValue));
        Assert.That(invoice.PaymentDate, Is.EqualTo(invoiceDto.PaymentDate));
        Assert.That(invoice.PaymentValue, Is.EqualTo(invoiceDto.PaymentValue));
        Assert.That(invoice.UpdatedAt, Is.EqualTo(invoiceDto.UpdatedAt));
    }
}