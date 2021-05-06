// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Innovt.Core.Attributes
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