// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
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