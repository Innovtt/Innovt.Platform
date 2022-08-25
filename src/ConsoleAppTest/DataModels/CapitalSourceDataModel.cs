#nullable enable
namespace ConsoleAppTest.DataModels;

public class CapitalSourceDataModel: BaseDataModel
{
    public string Name { get; set; }
    public string? CorporateName { get; set; }
    public bool Enabled { get; set; }
    public bool Deleted { get; set; }
    public bool IsExternalFund { get; set; }
    public string RootDocument { get; set; }
}