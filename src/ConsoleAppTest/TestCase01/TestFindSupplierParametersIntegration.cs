using System;

namespace ConsoleAppTest.TestCase01;

public class TestFindSupplierParametersIntegration : ITestParameters
{
    public string Id { get; set; }
    public string Document { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string Type { get; set; }

    public TestFindSupplierParametersIntegration()
    {
        Type = "FIND_SUPPLIER";
    }
}