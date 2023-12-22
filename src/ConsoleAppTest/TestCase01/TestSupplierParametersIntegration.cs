using System.Collections.Generic;

namespace ConsoleAppTest.TestCase01;

public class TestSupplierParametersIntegration
{
    public string AntecipaId { get; set; }
    public string Name { get; set; }
    public string RootDocument { get; set; }
    public string CorporateName { get; set; }
    public IDictionary<string, object> Metadata { get; set; }
}