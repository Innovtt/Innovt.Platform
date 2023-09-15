// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Linq;
using System.Reflection;

namespace Innovt.Core.Utilities;

/// <summary>
/// Provides simple object mapping functionality between two objects of different types.
/// </summary>
public static class SimpleMapper
{
    /// <summary>
    /// Maps the public properties with matching names and types from the input object to the output object.
    /// </summary>
    /// <typeparam name="T1">The type of the input object.</typeparam>
    /// <typeparam name="T2">The type of the output object.</typeparam>
    /// <param name="input">The input object to map from.</param>
    /// <param name="output">The output object to map to.</param>
    /// <returns>The output object with properties mapped from the input object.</returns>
    internal static T2 MapProperties<T1, T2>(T1 input, T2 output) where T1 : class
    {
        if (input == null)
            return default;

        var properties = input.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        var outputProperties = output.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        foreach (var property in properties)
        {
            var outProperty = outputProperties.LastOrDefault(p => p.Name == property.Name);

            if (outProperty != null && outProperty.PropertyType == property.PropertyType)
                outProperty.SetValue(output, property.GetValue(input, null), null);
        }

        return output;
    }

    /// <summary>
    /// Maps the properties from the input object to a new instance of the output object type.
    /// </summary>
    /// <typeparam name="T1">The type of the input object.</typeparam>
    /// <typeparam name="T2">The type of the output object.</typeparam>
    /// <param name="input">The input object to map from.</param>
    /// <returns>A new instance of the output object type with properties mapped from the input object.</returns>
    public static T2 Map<T1, T2>(T1 input) where T1 : class
    {
        if (input == null)
            return default;

        var output = Activator.CreateInstance<T2>();

        return MapProperties(input, output);
    }

    /// <summary>
    /// Maps the properties from the input object to the provided output object instance.
    /// </summary>
    /// <typeparam name="T1">The type of the input object.</typeparam>
    /// <typeparam name="T2">The type of the output object.</typeparam>
    /// <param name="inputInstance">The input object to map from.</param>
    /// <param name="outputInstance">The output object to map to.</param>
    public static void Map<T1, T2>(T1 inputInstance, T2 outputInstance) where T1 : class
    {
        if (inputInstance is null || outputInstance is null)
            return;

        MapProperties(inputInstance, outputInstance);
    }
}