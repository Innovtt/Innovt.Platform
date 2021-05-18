// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RequiredGuidAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            
            if (!Guid.TryParse(value.ToString(), out var result))
                return false;

            if (result == Guid.Empty)
                return false;


            return true;
        }
    }
}