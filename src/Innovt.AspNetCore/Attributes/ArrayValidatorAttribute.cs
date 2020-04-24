using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Innovt.AspNetCore.Attributes
{
    public class ArrayValidatorAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var valueType = value.GetType();

            if (!valueType.IsArray)
                return false;

            if (value is string[] list)
                return list.Count(s => s.Trim().Length == 0) == 0;

            return true;
        }
    }
}