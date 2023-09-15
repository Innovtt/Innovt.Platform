// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Utilities;

/// <summary>
/// Provides static methods for checking arguments and values.
/// </summary>
[DebuggerStepThrough]
public static class Check
{
    /// <summary>
    /// Checks if the specified value is not null; otherwise, throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="parameterName">The name of the parameter associated with the value.</param>
    /// <returns>The original value if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
    public static T NotNull<T>([AllowNull] [NotNull] T value, string parameterName)
    {
        if (value != null) return value;

        NotEmpty(parameterName, nameof(parameterName));

        throw new ArgumentNullException(parameterName);
    }

    /// <summary>
    /// Checks if the specified nullable integer value is greater than zero; otherwise, throws a <see cref="BusinessException"/>.
    /// </summary>
    /// <param name="value">The nullable integer value to check.</param>
    /// <param name="parameterName">The name of the parameter associated with the value.</param>
    /// <returns>Zero if the value is greater than zero.</returns>
    /// <exception cref="BusinessException">Thrown if the value is less than or equal to zero.</exception>
    public static int NotLessThanZero(int? value, string parameterName)
    {
        if (value.IsLessThanOrEqualToZero())
            throw new BusinessException(parameterName);

        return 0;
    }

    /// <summary>
    /// Checks if the specified integer value is greater than zero; otherwise, throws a <see cref="BusinessException"/>.
    /// </summary>
    /// <param name="value">The integer value to check.</param>
    /// <param name="parameterName">The name of the parameter associated with the value.</param>
    /// <returns>Zero if the value is greater than zero.</returns>
    /// <exception cref="BusinessException">Thrown if the value is less than or equal to zero.</exception>
    public static int NotLessThanZero(int value, string parameterName)
    {
        if (value.IsLessThanOrEqualToZero())
            throw new BusinessException(parameterName);

        return 0;
    }
    /// <summary>
    /// Checks if the specified array of integers are greater than zero; otherwise, throws a <see cref="BusinessException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the array elements, which must be convertible to an integer.</typeparam>
    /// <param name="value">The array of values to check.</param>
    /// <exception cref="BusinessException">Thrown if any of the values is less than or equal to zero.</exception>
    public static void NotLessThanZero<T>(params T[] value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        foreach (var i in value)
        {
            var item = i as int?;

            if (item.IsLessThanOrEqualToZero())
                throw new BusinessException(nameof(i));
        }
    }

    /// <summary>
    /// Checks if the specified value is not null; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="message">The message for the exception if the value is null.</param>
    /// <returns>The original value if it is not null.</returns>
    /// <exception cref="BusinessException">Thrown if the value is null, with the specified message.</exception>
    public static T NotNullWithBusinessException<T>([NotNull] T value, string message)
    {
        if (value == null) throw new BusinessException(message);

        return value;
    }
    /// <summary>
    /// Checks if the specified value is not null; otherwise, throws a <see cref="CriticalException"/> with the specified message.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="message">The message for the exception if the value is null.</param>
    /// <returns>The original value if it is not null.</returns>
    /// <exception cref="CriticalException">Thrown if the value is null, with the specified message.</exception>
    public static T NotNullWithCriticalException<T>([NotNull] T value, string message)
    {
        if (value == null) throw new CriticalException(message);

        return value;
    }

    /// <summary>
    /// Checks if two strings are equal while ignoring case sensitivity; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first string to compare.</param>
    /// <param name="value2">The second string to compare.</param>
    /// <param name="message">The message for the exception if the strings are not equal.</param>
    /// <exception cref="BusinessException">Thrown if the strings are not equal, ignoring case sensitivity, with the specified message.</exception>
    private static bool AreEqualImpl(string value, string value2)
    {
        return value != null && !value.Equals(value2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if two integer values are equal; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first integer value to compare.</param>
    /// <param name="value2">The second integer value to compare.</param>
    /// <param name="message">The message for the exception if the values are not equal.</param>
    /// <exception cref="BusinessException">Thrown if the values are not equal, with the specified message.</exception>
    public static void AreEqual(string value, string value2, string message)
    {
        if (AreEqualImpl(value, value2)) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if two long values are equal; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first long value to compare.</param>
    /// <param name="value2">The second long value to compare.</param>
    /// <param name="message">The message for the exception if the values are not equal.</param>
    /// <exception cref="BusinessException">Thrown if the values are not equal, with the specified message.</exception>
    public static void AreEqual(int value, int value2, string message)
    {
        if (value != value2) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if two decimal values are equal; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first decimal value to compare.</param>
    /// <param name="value2">The second decimal value to compare.</param>
    /// <param name="message">The message for the exception if the values are not equal.</param>
    /// <exception cref="BusinessException">Thrown if the values are not equal, with the specified message.</exception>
    public static void AreEqual(long value, long value2, string message)
    {
        if (value != value2) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if two decimal values are equal; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first decimal value to compare.</param>
    /// <param name="value2">The second decimal value to compare.</param>
    /// <param name="message">The message to include in the exception if the values are not equal.</param>
    /// <exception cref="BusinessException">Thrown if the two decimal values are not equal.</exception>
    public static void AreEqual(decimal value, decimal value2, string message)
    {
        if (value != value2) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if two strings are not equal while ignoring case sensitivity; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first string to compare.</param>
    /// <param name="value2">The second string to compare.</param>
    /// <param name="message">The message for the exception if the strings are equal.</param>
    /// <exception cref="BusinessException">Thrown if the strings are equal, ignoring case sensitivity, with the specified message.</exception>
    public static void AreNotEqual(string value, string value2, string message)
    {
        if (!AreEqualImpl(value, value2)) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if two integer values are not equal; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first integer value to compare.</param>
    /// <param name="value2">The second integer value to compare.</param>
    /// <param name="message">The message for the exception if the values are equal.</param>
    /// <exception cref="BusinessException">Thrown if the values are equal, with the specified message.</exception>
    public static void AreNotEqual(int value, int value2, string message)
    {
        if (value == value2) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if two long values are not equal; otherwise, throws a <see cref="BusinessException"/> with the specified message.
    /// </summary>
    /// <param name="value">The first long value to compare.</param>
    /// <param name="value2">The second long value to compare.</param>
    /// <param name="message">The message for the exception if the values are equal.</param>
    /// <exception cref="BusinessException">Thrown if the values are equal, with the specified message.</exception>
    public static void AreNotEqual(long value, long value2, string message)
    {
        if (value == value2) throw new BusinessException(message);
    }

    /// <summary>
    /// Checks if the specified collection is not null and not empty; otherwise, throws an <see cref="ArgumentException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the collection elements.</typeparam>
    /// <param name="value">The collection to check for null and emptiness.</param>
    /// <param name="parameterName">The name of the parameter associated with the collection.</param>
    /// <returns>The original collection if it is not null and not empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the collection is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the collection is empty.</exception>
    public static IReadOnlyList<T> NotEmpty<T>([NotNull] IReadOnlyList<T> value, string parameterName)
    {
        NotNull(value, parameterName);

        if (value.Count == 0)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException(parameterName);
        }

        return value;
    }

    /// <summary>
    /// Checks if the specified string is not null and not empty; otherwise, throws an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="value">The string to check for null and emptiness.</param>
    /// <param name="parameterName">The name of the parameter associated with the string.</param>
    /// <returns>The original string if it is not null and not empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the string is empty.</exception>
    public static string NotEmpty(string value, string parameterName = null)
    {
        Exception e = null;
        if (value is null)
            e = new ArgumentNullException(parameterName);
        else if (value.Trim().Length == 0) e = new ArgumentException(parameterName);

        if (e != null)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw e;
        }

        return value;
    }
    /// <summary>
    /// Checks if the specified string is null but not empty; otherwise, throws an <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="value">The string to check for null but not emptiness.</param>
    /// <param name="parameterName">The name of the parameter associated with the string.</param>
    /// <returns>The original string if it is not null but not empty.</returns>
    /// <exception cref="ArgumentException">Thrown if the string is null or empty.</exception>
    public static string NullButNotEmpty(string value, string parameterName)
    {
        if (value is not { Length: 0 }) return value;


        NotEmpty(parameterName, nameof(parameterName));

        throw new ArgumentException(parameterName);
    }
}