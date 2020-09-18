using System.Collections.Generic;
using System.Linq;

namespace Innovt.Cloud.Table
{
    public sealed class FilterCondition
    {
        public string AttributeName { get; set; }
        public ComparisonOperator Operator { get; set; }
        public object Value { get; set; }

        public FilterCondition()
        {
        }

        public FilterCondition(string attributeName, ComparisonOperator comparisionOperator, object value)
        {
            this.AttributeName = attributeName;
            this.Operator = comparisionOperator;
            this.Value = value;
        }


        public FilterCondition(string attributeName, ComparisonOperator comparisionOperator):this(attributeName,comparisionOperator,null)
        {  
      
        }

    
    }
}