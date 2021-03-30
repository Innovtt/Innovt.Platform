using System;
using System.Collections.Generic;
using System.Diagnostics;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Utilities
{
    public class AppServiceAbc
    {
        public void AddUser(object user)
        {
            Check.NotNull(user, nameof(user));
        }
    }


    [DebuggerStepThrough]
    public static class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
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
            foreach (var i in value)
            {
                var item = i as int?;

                if (item.IsLessThanOrEqualToZero())
                    throw new BusinessException(nameof(i));
            }
        }


        // [ContractAnnotation("=> halt")]
        public static T NotNullWithBusinessException<T>(T value, string message)
        {
            if (value == null) throw new BusinessException(message);

            return value;
        }

        // [ContractAnnotation("=> halt")]
        public static T NotNullWithCriticalException<T>(T value, string message)
        {
            if (value == null) throw new CriticalException(message);

            return value;
        }

        private static bool AreEqualImpl(string value, string value2)
        {
            return value != null && !value.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
        }

        //  [ContractAnnotation("=> halt")]
        public static void AreEqual(string value, string value2, string message)
        {
            if (AreEqualImpl(value, value2)) throw new BusinessException(message);
        }

        // [ContractAnnotation("=> halt")]
        public static void AreEqual(int value, int value2, string message)
        {
            if (value != value2) throw new BusinessException(message);
        }

        // [ContractAnnotation("=> halt")]
        public static void AreEqual(long value, long value2, string message)
        {
            if (value != value2) throw new BusinessException(message);
        }

        // [ContractAnnotation("=> halt")]
        public static void AreEqual(decimal value, decimal value2, string message)
        {
            if (value != value2) throw new BusinessException(message);
        }

        // [ContractAnnotation("=> halt")]
        public static void AreNotEqual(string value, string value2, string message)
        {
            if (!AreEqualImpl(value, value2)) throw new BusinessException(message);
        }

        //[ContractAnnotation("=> halt")]
        public static void AreNotEqual(int value, int value2, string message)
        {
            if (value == value2) throw new BusinessException(message);
        }

        // [ContractAnnotation("=> halt")]
        public static void AreNotEqual(long value, long value2, string message)
        {
            if (value == value2) throw new BusinessException(message);
        }


        public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(nameof(parameterName));
            }

            return value;
        }

        public static string NotEmpty(string value, string parameterName = null)
        {
            Exception e = null;
            if (value is null)
                e = new ArgumentNullException(parameterName);
            else if (value.Trim().Length == 0) e = new ArgumentException(nameof(parameterName));

            if (e != null)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw e;
            }

            return value;
        }

        public static string NullButNotEmpty(string value, string parameterName)
        {
            if (value != null
                && value.Length == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(nameof(parameterName));
            }

            return value;
        }
    }
}