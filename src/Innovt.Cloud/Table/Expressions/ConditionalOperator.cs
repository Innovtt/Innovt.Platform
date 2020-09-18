
using Innovt.Core.Utilities;

namespace Innovt.Cloud.Table
{
    public class ConditionalOperator : ConstantClass
    {

        public static readonly ConditionalOperator And = new ConditionalOperator("AND",0);

        public static readonly ConditionalOperator Or = new ConditionalOperator("OR",1);

        protected ConditionalOperator(string value,int id) : base(value)
        {
            this.Id = id;
        }

        public int Id { get; private set; }
    }
}
