// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Linq;
using System.Reflection;

namespace Innovt.Core.Utilities;

public static class SimpleMapper
{
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

    public static T2 Map<T1, T2>(T1 input) where T1 : class
    {
        if (input == null)
            return default;

        var output = Activator.CreateInstance<T2>();

        return MapProperties(input, output);
    }

    public static void Map<T1, T2>(T1 inputInstance, T2 outputInstance) where T1 : class
    {
        if (inputInstance is null || outputInstance is null)
            return;

        MapProperties(inputInstance, outputInstance);
    }
}