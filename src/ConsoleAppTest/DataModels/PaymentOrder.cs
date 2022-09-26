using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;

namespace ConsoleAppTest.DataModels
{
    public class PaymentOrder : ValueObject<Guid>
    {
        public Guid InvoiceId { get; set; }

        public string ErpId { get; set; }

        public string LockToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int PaymentTypeId { get; set; }

        public int PaymentOrderStatusId { get; set; }

        //public PaymentOrderType PaymentOrderType { get; set; }

        public string PaymentOrderStatusReason { get; set; }

        public int CurrencyId { get; set; }

        public string InstallmentNumber { get; set; } = "1";

        public DateTime OriginalDueDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public decimal TotalValue { get; set; }

        public decimal Discount { get; set; }

        public decimal Interest { get; set; }

        public decimal Fine { get; set; }

        public decimal Tax { get; set; }

        public decimal NetValue { get; set; }

        public string AnticipationId { get; set; }

        public DateTime? AnticipationDate { get; set; }

        public decimal? AnticipationDiscount { get; set; }

        public int? AnticipatedDays { get; set; }

        public DateTime? AnticipatedAt { get; set; }

        public DateTime? PaymentForecastDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? PaymentValue { get; set; }

        public string BankSlipBarCode { get; set; }

        public string Metadata { get; set; }

        public int? OldId { get; set; }

        public PaymentOrder()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public decimal CalculateNetValue()
        {
            return TotalValue - (Discount + Interest +
                                 Fine + Tax);
        }


        private void ForceRelease()
        {
            LockToken = null;
        }

        public bool CheckIfHasLock()
        {
            return !string.IsNullOrWhiteSpace(LockToken);
        }

        public override bool Equals(object obj)
        {
            var order = obj as PaymentOrder;

            if (order == null) return false;

            return
                ErpId == order.ErpId &&
                PaymentTypeId == order.PaymentTypeId &&
                PaymentOrderStatusId == order.PaymentOrderStatusId &&
                PaymentOrderStatusReason == order.PaymentOrderStatusReason &&
                CurrencyId == order.CurrencyId &&
                InstallmentNumber == order.InstallmentNumber &&
                OriginalDueDate == order.OriginalDueDate &&
                DueDate == order.DueDate &&
                TotalValue == order.TotalValue &&
                Discount == order.Discount &&
                Interest == order.Interest &&
                Fine == order.Fine &&
                Tax == order.Tax &&
                NetValue == order.NetValue &&
                PaymentForecastDate == order.PaymentForecastDate &&
                PaymentDate == order.PaymentDate &&
                PaymentValue == order.PaymentValue &&
                (Description?.Trim() ?? "") == (order.Description?.Trim() ?? "") &&
                (BankSlipBarCode?.Trim() ?? "") == (order.BankSlipBarCode?.Trim() ?? "") &&
                (Metadata?.Trim() ?? "") == (order.Metadata?.Trim() ?? "");
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(InvoiceId);
            hash.Add(ErpId);
            hash.Add(PaymentTypeId);
            hash.Add(PaymentOrderStatusId);
            hash.Add(PaymentOrderStatusReason);
            hash.Add(CurrencyId);
            hash.Add(InstallmentNumber);
            hash.Add(OriginalDueDate);
            hash.Add(DueDate);
            hash.Add(Description);
            hash.Add(TotalValue);
            hash.Add(Discount);
            hash.Add(Interest);
            hash.Add(Fine);
            hash.Add(Tax);
            hash.Add(NetValue);
            hash.Add(PaymentForecastDate);
            hash.Add(PaymentDate);
            hash.Add(PaymentValue);
            hash.Add(BankSlipBarCode);
            hash.Add(Metadata);
            return hash.ToHashCode();
        }
    }
}