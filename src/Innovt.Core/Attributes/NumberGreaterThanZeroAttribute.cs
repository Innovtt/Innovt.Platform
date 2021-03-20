using System;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class RequiredNumberGreaterThanZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && int.TryParse(value.ToString(), out var i) && i > 0;
        }
    }
}