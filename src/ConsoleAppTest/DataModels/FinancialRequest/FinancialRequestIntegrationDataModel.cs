using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime.Internal;
using ConsoleAppTest.DataModels.CapitalSource.DataModels;
using Innovt.Domain.Adresses;
using Innovt.Domain.Contacts;

namespace ConsoleAppTest.DataModels.FinancialRequest
{
    public class FinancialRequestIntegrationDataModel : BaseIntegrationDataModel
    {
        public FinancialRequestIntegrationDataModel()
        {
            EntityType = "FinancialRequest";
            RequestedAt = DateTime.UtcNow;
        }

        public int Provider { get; set; }

        public SignDataModel Sign { get; set; }

        public Guid? SignId { get; set; }

        public ContractDataModel Contract { get; set; }

        public List<InvoiceDataModel> Invoices { get; set; }

        public BuyerDataModel Buyer { get; set; }

        public CapitalSourceDataModel CapitalSource { get; set; }

        public int StatusId { get; set; }

        public List<FinancialRequestStatusHistoryDataModel> StatusHistory { get; private set; }

        public DateTime NextWeekDay { get; set; }

        public Guid FinancialRequestId { get; set; }

        public string PayloadFilePath { get; set; }

        public long SequenceNumber { get; set; }

        public string TransactionalFileName { get; set; }

        public string ResponsePayloadFilePath { get; set; }

        public DateTime OperationDate { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime RequestedAt { get; set; }

        public UserDataModel RequestedByUser { get; set; }

        public int FinancialStrategy { get; set; }
        public string EntityTypeRequestedAt { get => $"ET#{EntityType}#RDT#{RequestedAt:yyyy-MM-ddTHH:mm:ss.000Z}"; set => _ = value; }

     
    }

    public class UserDataModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }

    public class SignDataModel
    {
        public Guid SignId { get; set; }

        public Guid? SignTemplateId { get; set; }

        public string SignProvider { get; set; }

        public string SignMessage { get; set; }
        public DateTime? SignedAt { get; set; }
        public DateTime? CanceledAt { get; set; }

        public List<SignerDataModel> Signers { get; set; }

        public Dictionary<string, string> Payload { get; set; }

    }

    public class SignerDataModel
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Document { get; set; }
    }

    public class InvoiceDataModel
    {
        public Guid Id { get; set; }

        public decimal TotalValue { get; set; }

        public decimal NetValue { get; set; }

        public decimal IofValue { get; set; }

        public int InvoiceNumber { get; set; }

        public string InvoiceLetter { get; set; }

        public DateTime IssueDate { get; set; }

        public int DaysToDue { get; set; }

        public int OrderId { get; set; }

        public string NamePaymentType { get; set; }

        public string NamePaymentOrderStatus { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Value { get; set; }

        public decimal Discount { get; set; }

        public decimal NetValuePaymentOrder { get; set; }

        public decimal InstallmentNumber { get; set; }

        public string BankSlipBarCode { get; set; }

        public decimal PaymentValue { get; set; }


        public decimal CapitalSourceFee { get; set; }

        public decimal BuyerFee { get; set; }

        public decimal AntecipaFee { get; set; }

        public decimal Interest { get; set; }

        public DateTime OperationDate { get; set; }

        public decimal Taxes { get; set; }

        public int FiscalId { get; set; }

        public string FiscalNumber { get; set; }

        public string FiscalSeries { get; set; }

        public DateTime FiscalIssueDate { get; set; }

        public decimal FiscalValue { get; set; }

        public SupplierDataModel Supplier { get; set; }

        public Guid SupplierId { get; set; }

        public int MediumTerm { get; set; }

        public bool CanAnticipate { get; set; }

        public long SequenceNumber { get; set; }

        public bool HasProcessingErrors { get; set; }

        public string ProcessingErrorsDescription { get; set; }

        public bool IsPaid { get; set; }

        public string FiscalAccessKey { get; set; }
        public int TotalNumberOfPaymentOrders { get; set; }
       
    }

    public class SupplierDataModel
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string CorporateName { get; set; }

        public AddressDataModel Address { get; set; }

        public string Document { get; set; }

        public Phone Phone { get; set; }

        public int ContactId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Activity { get; set; }

        public BankDataModel Bank { get; set; }

        public int CaixaPostal { get; set; }

        public int BankAccountType { get; set; }


        public string FavoredDocument { get; set; }

        public string FavoredName { get; set; }

        public List<SupplierPartnerDataModel> Partners { get; set; }
        public Guid Id { get; set; }
    }

    public class SupplierPartnerDataModel
    {
        public string Document { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class BankDataModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string RoutingNumber { get; set; }

        public string RoutingDigit { get; set; }

        public string AccountNumber { get; set; }

        public string AccountDigit { get; set; }

    }

    public class AddressDataModel
    {
        public string Description { get; set; }

        public string Street { get; set; }

        public string Complement { get; set; }

        public string Neighborhood { get; set; }

        public string Number { get; set; }

        public string ZipCode { get; set; }

        public CityDataModel City { get; set; }
    }

    public class CityDataModel
    {
        public string Name { get; set; }

        public StateDataModel State { get; set; }

    }

    public class StateDataModel
    {
        public string Description { get; set; }

        public string Acronym { get; set; }

        public string Country { get; set; }
    }

    public class BuyerDataModel
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string CorporateName { get; set; }

        public BankDataModel Bank { get; set; }

        public string Document { get; set; }

        public BuyerAddressDataModel Address { get; set; }

    }


    public class BuyerAddressDataModel
    {
        public string Street { get; set; }

        public string Neighborhood { get; set; }

        public string Number { get; set; }

        public string ZipCode { get; set; }

        public string CountryCode { get; set; }

        public string StateCode { get; set; }

        public string CityName { get; set; }

        public string Complement { get; set; }

    }
    public class FinancialRequestStatusHistoryDataModel
    {
        public int FinancialRequestStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}