using System;

namespace ConsoleAppTest.DataModels.CapitalSource.DataModels
{
    public class ContractParametersDataModel
    {
        public string Number { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountDigit { get; set; }
        public string BankAgency { get; set; }
        public decimal CapitalCost { get; set; }
        public decimal CutValue { get; set; }
        public int CutRateType { get; set; }
        public decimal CutRate { get; set; }
        public decimal OperationFee { get; set; }
        public decimal Rebate { get; set; }
        public decimal TitleFee { get; set; }
        public string AgreementNumber { get; set; }
        public string RegistrationInstructions { get; set; }
        public int MinimumDaysToDue { get; set; }
        public DateTime? OperationLimitDate { get; set; }
        public int DaysToPayment { get; set; }
        public int? CreditType { get; set; }
        public long? MarketOpeningTime { get; set; }
        public long? MarketClosingTime { get; set; }
        public decimal AverageReturnRate { get; set; }
    }
}