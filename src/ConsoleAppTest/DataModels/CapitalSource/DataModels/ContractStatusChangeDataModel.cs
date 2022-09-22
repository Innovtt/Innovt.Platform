using System;

namespace ConsoleAppTest.DataModels.CapitalSource.DataModels
{
    public class ContractStatusChangeDataModel
    {
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public int StatusId { get; set; }
    }
}