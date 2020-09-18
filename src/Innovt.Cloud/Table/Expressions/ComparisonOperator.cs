
using Innovt.Core.Utilities;

namespace Innovt.Cloud.Table
{
    public class ComparisonOperator: ConstantClass
    {
        public int Id { get; set; }
        protected ComparisonOperator(string value,int id):base(value)
        {
            this.Id = id;
        }

        public static readonly ComparisonOperator Equal = new ComparisonOperator("EQ", 0);

        public static readonly ComparisonOperator NotEqual = new ComparisonOperator("NE", 1);

        public static readonly ComparisonOperator LessThanOrEqual = new ComparisonOperator("LE", 2);

        public static readonly ComparisonOperator LessThan = new ComparisonOperator("LT", 3);

        public static readonly ComparisonOperator GreaterThanOrEqual = new ComparisonOperator("GE", 4);

        public static readonly ComparisonOperator GreaterThan = new ComparisonOperator("GT", 5);

        public static readonly ComparisonOperator NotNull = new ComparisonOperator("NOT_NULL", 6);

        public static readonly ComparisonOperator Null = new ComparisonOperator("NULL", 7);

        public static readonly ComparisonOperator Contains = new ComparisonOperator("CONTAINS", 8);

        public static readonly ComparisonOperator NotContains = new ComparisonOperator("NOT_CONTAINS", 9);

        public static readonly ComparisonOperator BeginsWith = new ComparisonOperator("BEGINS_WITH", 10);

        public static readonly ComparisonOperator In = new ComparisonOperator("IN", 11);

        public static readonly ComparisonOperator Between = new ComparisonOperator("BETWEEN",12);
    }
}
