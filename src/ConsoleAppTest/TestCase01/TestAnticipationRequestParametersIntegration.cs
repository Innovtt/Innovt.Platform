using System;
using System.Collections.Generic;

namespace ConsoleAppTest.TestCase01;

public class TestAnticipationRequestParametersIntegration : ITestParameters
{
    public string Type { get; set; }
    public string AntecipaId { get; set; }
    public string CapitalSourceId { get; set; }
    public DateTime AnticipationDate { get; set; }
    public TestSupplierParametersIntegration Supplier { get; set; }
    public List<TestAnticipatePaymentOrdersParametersIntegration> AnticipatePaymentOrders { get; set; }
    public IDictionary<string, object> Metadata { get; set; }

    public TestAnticipationRequestParametersIntegration()
    {
        Type = "ANTICIPATE_PAYMENTS";
    }
}