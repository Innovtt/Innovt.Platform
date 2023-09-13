// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Innovt.Core.Attributes;
/// <summary>
/// Custom validation attribute for arrays.
/// </summary>
/// <remarks>
/// This attribute is used to validate whether an object is an array and, in the case of string arrays,
/// whether any of the strings in the array are empty or contain only whitespace.
/// </remarks>
public sealed class ArrayValidatorAttribute : RequiredAttribute
{
    /// <summary>
    /// Determines whether the specified value is a valid array.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>
    ///   <c>true</c> if the value is a valid array; otherwise, <c>false</c>.
    /// </returns>
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        var valueType = value.GetType();

        if (!valueType.IsArray)
            return false;

        if (value is string[] list)
            return list.Any(s => s.Trim().Length == 0);

        return true;
    }
}