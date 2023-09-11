// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
/// <summary>
/// Attribute used to validate that a property or field contains a non-empty Guid value.
/// </summary>
/// <remarks>
/// This attribute is applied to properties or fields and is used to ensure that the value
/// is a non-empty Guid. It can be used for data validation in various scenarios.
/// </remarks>
public sealed class RequiredGuidAttribute : RequiredAttribute
{
    /// <summary>
    /// Determines whether the specified value is a non-empty Guid.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>
    ///   <c>true</c> if the value is a non-empty Guid; otherwise, <c>false</c>.
    /// </returns>
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