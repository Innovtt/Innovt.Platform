using System;

namespace ConsoleAppTest.TestCase01
{
    public class TestFindFinancialDocumentParametersIntegration : ITestPagedIntervalDateParametersIntegration
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string SupplierDocument { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Type { get; set; }

        public TestFindFinancialDocumentParametersIntegration()
        {
            Type = "FIND_FINANCIAL_DOCUMENT";
        }
    }
}