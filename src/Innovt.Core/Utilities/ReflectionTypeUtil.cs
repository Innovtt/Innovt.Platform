using System;
using System.Linq;
using System.Linq.Expressions;

namespace Innovt.Core.Utilities;

public static class ReflectionTypeUtil
{
    /// <summary>
    ///     Perform a cache of the compiled expression to create instances of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Func<T> CreateInstance<T>() where T : class
    {
        // Use compiled expression to create instances of T
        var ctor = typeof(T).GetConstructor(Type.EmptyTypes);

        if (ctor == null)
            throw new InvalidOperationException($"Type {typeof(T)} does not have a parameterless constructor.");

        var newExpr = Expression.New(ctor);
        var lambda = Expression.Lambda<Func<T>>(newExpr);

        return lambda.Compile();
    }

    public static Func<object> CreateInstance(Type type)
    {
        // Use compiled expression to create instances of T
        var ctor = type.GetConstructor(Type.EmptyTypes);

        if (ctor == null)
            throw new InvalidOperationException($"Type {type} does not have a parameterless constructor.");

        var newExpr = Expression.New(ctor);
        var lambda = Expression.Lambda<Func<object>>(newExpr);

        return lambda.Compile();
    }

    public static Func<object> CreateInstance(Type type, params object[] args)
    {
        // Use compiled expression to create instances of T
        var ctor = type.GetConstructor(args.Select(a => a.GetType()).ToArray());

        if (ctor == null)
            throw new InvalidOperationException(
                $"Type {type} does not have a constructor with the specified parameters.");

        var newExpr = Expression.New(ctor, args.Select(Expression.Constant));
        var lambda = Expression.Lambda<Func<Type>>(newExpr);

        return lambda.Compile();
    }
}