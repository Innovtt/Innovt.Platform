using System;
using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Innovt.Cloud.AWS.Dynamo.Converters;

public class DateTimeOffsetConverter : IPropertyConverter
{
    public DynamoDBEntry ToEntry(object value)
    {
        if (value is null)
            return new DynamoDBNull();

        return new Primitive(((DateTimeOffset)value).ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    ///     Here is from DateTimeOffSet to Date
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public object FromEntry(DynamoDBEntry entry)
    {   
        if (entry is DynamoDBNull || entry is null)
            return null;

        if (DateTimeOffset.TryParse(entry.ToString(),CultureInfo.InvariantCulture, 
                out var value))
            return value;
        
        if (DateTime.TryParse(entry.ToString(),CultureInfo.InvariantCulture, out var valueDate))
            return valueDate;

        return null;
    }
}