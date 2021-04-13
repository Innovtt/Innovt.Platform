// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredNumberGreaterThanZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && int.TryParse(value.ToString(), out var i) && i > 0;
        }
    }
}