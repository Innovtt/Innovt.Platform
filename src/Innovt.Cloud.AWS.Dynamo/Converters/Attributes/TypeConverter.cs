using System;
using System.ComponentModel;
using System.Globalization;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes;

internal static class TypeConverter
{
    /// <summary>
    ///     Converts a value to the specified property type, considering type conversion and compatibility workarounds.
    /// </summary>
    /// <param name="propertyType">The target property type to convert the value to.</param>
    /// <param name="value">The value to be converted.</param>
    /// <returns>
    ///     The converted value of the specified property type, or the default value of the property type if the input value is
    ///     null.
    /// </returns>

    internal static object ConvertType(Type propertyType, object value)   
    {
        if (value is null)
            return default;

        var typeConverter = TypeDescriptor.GetConverter(propertyType);

        if (typeConverter.CanConvertFrom(value.GetType()))
        {
            //workaround compatibility v1 and v2
            if (typeConverter is BooleanConverter && (value.ToString() == "1" || value.ToString() == "0"))
                return typeConverter.ConvertFrom(null!, CultureInfo.InvariantCulture,
                    value.ToString() == "1" ? "true" : "false");

            return typeConverter.ConvertFrom(null!, CultureInfo.InvariantCulture, value);
        }

        var destinationType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        return Convert.ChangeType(value, destinationType, CultureInfo.InvariantCulture);
    }
}