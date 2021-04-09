namespace Innovt.Cloud.Table
{
    public class BaseRequest
    {
        public string IndexName { get; set; }

        public object Filter { get; set; }

        public string AttributesToGet { get; set; }
        public int? PageSize { get; set; }
        public string Page { get; set; }
        public string FilterExpression { get; set; }
    }
}