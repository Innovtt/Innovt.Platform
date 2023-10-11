// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;

namespace Innovt.Core.Utilities;

/// <summary>
/// Represents a base class for constants with a string value.
/// </summary>
public class ConstantClass
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstantClass"/> class with the specified value.
    /// </summary>
    /// <param name="value">The constant value as a string.</param>
    protected ConstantClass(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the constant value as a string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Determines whether this instance of <see cref="ConstantClass"/> is equal to another object by comparing their string values.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        return obj?.ToString() == Value;
    }

    /// <summary>
    /// Determines whether this instance of <see cref="ConstantClass"/> is equal to another <see cref="ConstantClass"/> by comparing their string values.
    /// </summary>
    /// <param name="obj">The <see cref="ConstantClass"/> to compare with this instance.</param>
    /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(ConstantClass obj)
    {
        return obj?.Value == Value;
    }

    /// <summary>
    /// Returns the hash code for this instance of <see cref="ConstantClass"/>.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Compares a <see cref="ConstantClass"/> instance and a string for equality by comparing their string values.
    /// </summary>
    /// <param name="a">The <see cref="ConstantClass"/> to compare.</param>
    /// <param name="b">The string to compare.</param>
    /// <returns><c>true</c> if the <see cref="ConstantClass"/> and string are equal; otherwise, <c>false</c>.</returns>
    protected virtual bool Equals(string value)
    {
        return Value == value;
    }

    /// <summary>
    /// Compares a <see cref="ConstantClass"/> instance and a string for equality by comparing their string values.
    /// </summary>
    /// <param name="a">The <see cref="ConstantClass"/> to compare.</param>
    /// <param name="b">The string to compare.</param>
    /// <returns><c>true</c> if the <see cref="ConstantClass"/> and string are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(ConstantClass a, ConstantClass b)
    {
        return a?.Value == b?.Value;
    }

    /// <summary>
    /// Compares a string and a <see cref="ConstantClass"/> instance for equality by comparing their string values.
    /// </summary>
    /// <param name="a">The string to compare.</param>
    /// <param name="b">The <see cref="ConstantClass"/> to compare.</param>
    /// <returns><c>true</c> if the string and <see cref="ConstantClass"/> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(ConstantClass a, string b)
    {
        return a?.Value == b;
    }

    /// <summary>
    /// Compares two <see cref="ConstantClass"/> instances for inequality by comparing their string values.
    /// </summary>
    /// <param name="a">The first <see cref="ConstantClass"/> to compare.</param>
    /// <param name="b">The second <see cref="ConstantClass"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="ConstantClass"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(string a, ConstantClass b)
    {
        return a == b?.Value;
    }

    /// <summary>
    /// Compares a <see cref="ConstantClass"/> instance and a string for inequality by comparing their string values.
    /// </summary>
    /// <param name="a">The <see cref="ConstantClass"/> to compare.</param>
    /// <param name="b">The string to compare.</param>
    /// <returns><c>true</c> if the <see cref="ConstantClass"/> and string are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(ConstantClass a, ConstantClass b)
    {
        return a?.Value != b?.Value;
    }

    /// <summary>
    /// Compares a string and a <see cref="ConstantClass"/> instance for inequality by comparing their string values.
    /// </summary>
    /// <param name="a">The string to compare.</param>
    /// <param name="b">The <see cref="ConstantClass"/> to compare.</param>
    /// <returns><c>true</c> if the string and <see cref="ConstantClass"/> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(ConstantClass a, string b)
    {
        return a?.Value != b;
    }

    /// <summary>
    /// Compares a string and a <see cref="ConstantClass"/> instance for inequality by comparing their string values.
    /// </summary>
    /// <param name="a">The string to compare.</param>
    /// <param name="b">The <see cref="ConstantClass"/> to compare.</param>
    /// <returns><c>true</c> if the string and <see cref="ConstantClass"/> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(string a, ConstantClass b)
    {
        return a != b?.Value;
    }

    /// <summary>
    /// Implicitly converts a <see cref="ConstantClass"/> instance to a string by returning its string value.
    /// </summary>
    /// <param name="value">The <see cref="ConstantClass"/> instance to convert.</param>
    /// <returns>The string value of the <see cref="ConstantClass"/> instance.</returns>
    public static implicit operator string(ConstantClass value)
    {
        return value?.Value;
    }
}