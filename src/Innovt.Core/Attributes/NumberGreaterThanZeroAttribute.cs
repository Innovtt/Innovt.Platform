// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Attributes;

/// <summary>
/// Attribute used to validate that a numeric property, field, or parameter is greater than zero.
/// </summary>
/// <remarks>
/// This attribute is applied to properties, fields, or parameters and is used to ensure that the value
/// is a numeric value greater than zero. It can be used for data validation in various scenarios.
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class RequiredNumberGreaterThanZeroAttribute : ValidationAttribute
{
    /// <summary>
    /// Determines whether the specified value is a numeric value greater than zero.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>
    ///   <c>true</c> if the value is a numeric value greater than zero; otherwise, <c>false</c>.
    /// </returns>
    public override bool IsValid(object value)
    {
        return value != null && int.TryParse(value.ToString(), out var i) && i > 0;
    }
}