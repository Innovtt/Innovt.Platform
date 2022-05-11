// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Utilities;

[DebuggerStepThrough]
public static class Check
{
    public static T NotNull<T>([AllowNull] [NotNull] T value, string parameterName)
    {
        if (value != null) return value;

        NotEmpty(parameterName, nameof(parameterName));

        throw new ArgumentNullException(parameterName);
    }

    public static int NotLessThanZero(int? value, string parameterName)
    {
        if (value.IsLessThanOrEqualToZero())
            throw new BusinessException(parameterName);

        return 0;
    }


    public static int NotLessThanZero(int value, string parameterName)
    {
        if (value.IsLessThanOrEqualToZero())
            throw new BusinessException(parameterName);

        return 0;
    }

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

    public static T NotNullWithBusinessException<T>([NotNull] T value, string message)
    {
        if (value == null) throw new BusinessException(message);

        return value;
    }

    public static T NotNullWithCriticalException<T>([NotNull] T value, string message)
    {
        if (value == null) throw new CriticalException(message);

        return value;
    }

    private static bool AreEqualImpl(string value, string value2)
    {
        return value != null && !value.Equals(value2, StringComparison.OrdinalIgnoreCase);
    }

    public static void AreEqual(string value, string value2, string message)
    {
        if (AreEqualImpl(value, value2)) throw new BusinessException(message);
    }

    public static void AreEqual(int value, int value2, string message)
    {
        if (value != value2) throw new BusinessException(message);
    }

    public static void AreEqual(long value, long value2, string message)
    {
        if (value != value2) throw new BusinessException(message);
    }


    public static void AreEqual(decimal value, decimal value2, string message)
    {
        if (value != value2) throw new BusinessException(message);
    }


    public static void AreNotEqual(string value, string value2, string message)
    {
        if (!AreEqualImpl(value, value2)) throw new BusinessException(message);
    }


    public static void AreNotEqual(int value, int value2, string message)
    {
        if (value == value2) throw new BusinessException(message);
    }


    public static void AreNotEqual(long value, long value2, string message)
    {
        if (value == value2) throw new BusinessException(message);
    }


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

    public static string NullButNotEmpty(string value, string parameterName)
    {
        if (value is not { Length: 0 }) return value;


        NotEmpty(parameterName, nameof(parameterName));

        throw new ArgumentException(parameterName);
    }
}