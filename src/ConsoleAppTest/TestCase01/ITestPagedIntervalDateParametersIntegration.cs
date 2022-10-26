using System;

namespace ConsoleAppTest.TestCase01
{
    public interface ITestPagedIntervalDateParametersIntegration : ITestParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}