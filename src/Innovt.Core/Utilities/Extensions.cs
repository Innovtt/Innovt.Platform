// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Innovt.Core.Utilities;

/// <summary>
///     Provides extension methods for various data types and operations.
/// </summary>
public static class Extensions
{
    private static readonly ConcurrentDictionary<Type, bool> IsSimpleTypeCache =
        new();

    /// <summary>
    ///     Determines whether the specified object is null.
    /// </summary>
    /// <param name="obj">The object to check for null.</param>
    /// <returns>True if the object is null; otherwise, false.</returns>
    public static bool IsNull([NotNullWhen(false)] this object obj)
    {
        return obj == null;
    }

    /// <summary>
    ///     Converts the specified object to a string or returns an empty string if the object is null.
    /// </summary>
    /// <param name="obj">The object to convert to a string.</param>
    /// <returns>The string representation of the object or an empty string if the object is null.</returns>
    public static string ToStringOrDefault(this object obj)
    {
        return obj == null ? string.Empty : obj.ToString();
    }

    /// <summary>
    ///     Determines whether the specified Guid is empty.
    /// </summary>
    /// <param name="id">The Guid to check for emptiness.</param>
    /// <returns>True if the Guid is empty; otherwise, false.</returns>
    public static bool IsGuidEmpty(this Guid id)
    {
        return id == Guid.Empty;
    }

    /// <summary>
    ///     Determines whether the specified nullable Guid is null or empty.
    /// </summary>
    /// <param name="id">The nullable Guid to check for null or emptiness.</param>
    /// <returns>True if the Guid is null or empty; otherwise, false.</returns>
    public static bool IsGuidNullOrEmpty([NotNullWhen(false)] this Guid? id)
    {
        return id is null || id.Value.IsGuidEmpty();
    }

    /// <summary>
    ///     Determines whether the specified integer is less than or equal to zero.
    /// </summary>
    /// <param name="id">The integer to check.</param>
    /// <returns>True if the integer is less than or equal to zero; otherwise, false.</returns>
    public static bool IsLessThanOrEqualToZero(this int id)
    {
        return id <= 0;
    }

    /// <summary>
    ///     Determines whether the specified nullable integer is null or less than or equal to zero.
    /// </summary>
    /// <param name="id">The nullable integer to check.</param>
    /// <returns>True if the integer is null or less than or equal to zero; otherwise, false.</returns>
    public static bool IsLessThanOrEqualToZero(this int? id)
    {
        return id.GetValueOrDefault().IsLessThanOrEqualToZero();
    }

    /// <summary>
    ///     Determines whether the specified long integer is less than or equal to zero.
    /// </summary>
    /// <param name="id">The long integer to check.</param>
    /// <returns>True if the long integer is less than or equal to zero; otherwise, false.</returns>
    public static bool IsLessThanOrEqualToZero(this long id)
    {
        return id <= 0;
    }

    /// <summary>
    ///     Determines whether the specified double is less than or equal to zero.
    /// </summary>
    /// <param name="id">The double to check.</param>
    /// <returns>True if the double is less than or equal to zero; otherwise, false.</returns>
    public static bool IsLessThanOrEqualToZero(this double id)
    {
        return id <= 0;
    }

    //From Stack Owverlow: https://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive
    /// <summary>
    ///     Determines whether the specified type is a primitive data type.
    /// </summary>
    /// <param name="type">The Type to check.</param>
    /// <returns>True if the type is a primitive data type; otherwise, false.</returns>
    public static bool IsPrimitiveType(this Type type)
    {
        return IsSimpleTypeCache.GetOrAdd(type, t =>
            type.IsPrimitive ||
            type.IsEnum ||
            type == typeof(string) ||
            type == typeof(decimal) ||
            type == typeof(DateTime) ||
            type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan) ||
            type == typeof(Guid) ||
            IsNullableSimpleType(type));

        static bool IsNullableSimpleType(Type t)
        {
            var underlyingType = Nullable.GetUnderlyingType(t);
            return underlyingType != null && IsPrimitiveType(underlyingType);
        }
    }

    /// <summary>
    ///     Extracts latitude and longitude coordinates from a string.
    /// </summary>
    /// <param name="str">The string containing coordinates.</param>
    /// <param name="splittedBy">The character used to split latitude and longitude values.</param>
    /// <returns>A tuple containing latitude and longitude values.</returns>
    public static (long Latitude, long Longitude) ExtractCoordinates(this string str, char splittedBy = ';')
    {
        var splittedValues = str?.Split(splittedBy);

        if (splittedValues == null || splittedValues.Length == 0)
            return (0, 0);

        long latitude = 0;

        if (!splittedValues[0].IsNullOrEmpty())
        {
            var op = splittedValues[0][0] == '-' ? -1 : 1;

            long.TryParse(splittedValues[0].OnlyNumber(), out latitude);

            latitude *= op;
        }


        long longitude = 0;

        if (splittedValues.Length > 1 && !splittedValues[1].IsNullOrEmpty())
        {
            var op = splittedValues[1][0] == '-' ? -1 : 1;

            long.TryParse(splittedValues[1].OnlyNumber(), out longitude);

            longitude *= op;
        }

        return (latitude, longitude);
    }

    /// <summary>
    ///     Converts a double value representing milliseconds since the Unix epoch to a DateTimeOffset.
    /// </summary>
    /// <param name="fromSeconds">The double value representing milliseconds since the Unix epoch.</param>
    /// <returns>A DateTimeOffset representing the converted date and time.</returns>
    public static DateTimeOffset? MillisecondToDateTime(this double fromSeconds)
    {
        var dateTime = DateTimeOffset.MinValue;
        dateTime = dateTime.AddSeconds(fromSeconds).ToLocalTime();
        return dateTime;
    }

    /// <summary>
    ///     Formats a decimal value as currency.
    /// </summary>
    /// <param name="value">The decimal value to format.</param>
    /// <returns>A string representation of the decimal value in currency format.</returns>
    public static string FormatToCurrency(this decimal value)
    {
        return $"{value:C}";
    }

    /// <summary>
    ///     Returns the last day of the month for a given DateTimeOffset.
    /// </summary>
    /// <param name="date">The DateTimeOffset for which to find the last day of the month.</param>
    /// <returns>The DateTimeOffset representing the last day of the month.</returns>
    public static DateTimeOffset LastDayOfMonth(this DateTimeOffset date)
    {
        return new DateTimeOffset(
            new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).AddDays(1)
                .AddMilliseconds(-1), date.Offset);
    }

    /// <summary>
    ///     Formats a DateTimeOffset as a human-readable date and time string.
    /// </summary>
    /// <param name="value">The DateTimeOffset to format.</param>
    /// <returns>A string representation of the DateTimeOffset in a custom format.</returns>
    public static string FormatToExtensionDateTime(this DateTimeOffset value)
    {
        var weekDay = value.ToString("dddd", CultureInfo.DefaultThreadCurrentCulture).Split('-');

        var dia = weekDay[0][..1].ToUpper(CultureInfo.DefaultThreadCurrentCulture) + weekDay[0][1..];

        if (weekDay.Length == 2)
            dia += "-" + weekDay[1][..1].ToUpper(CultureInfo.DefaultThreadCurrentCulture) + weekDay[1][1..];

        var mes = value.ToString("MMMM", CultureInfo.DefaultThreadCurrentCulture);

        mes = mes[..1].ToUpper(CultureInfo.DefaultThreadCurrentCulture) + mes[1..];

        return dia + ", " + value.ToString("dd", CultureInfo.DefaultThreadCurrentCulture) + " de " + mes + " de " +
               value.ToString("yyyy", CultureInfo.DefaultThreadCurrentCulture) + ", " +
               value.ToString("t", CultureInfo.DefaultThreadCurrentCulture) + "h";
    }

    /// <summary>
    ///     Formats a DateTimeOffset as a simple date and time string.
    /// </summary>
    /// <param name="value">The DateTimeOffset to format.</param>
    /// <returns>A string representation of the DateTimeOffset in a simple format.</returns>
    public static string FormatToSimpleDateTime(this DateTimeOffset value)
    {
        var weekDay = value.ToString("dddd").Split('-');

        var dia = weekDay[0][..1].ToUpper() + weekDay[0][1..];

        return dia + " - " + value.Day + "/" + value.Month;
    }

    /// <summary>
    ///     Formats a date period between two DateTimes as a string.
    /// </summary>
    /// <param name="startDate">The start date of the period.</param>
    /// <param name="endDate">The end date of the period.</param>
    /// <param name="cultureInfo">Optional CultureInfo for formatting. Defaults to null.</param>
    /// <returns>A string representing the formatted date period.</returns>
    public static string FormatToPeriod(this DateTime startDate, DateTime endDate, CultureInfo cultureInfo = null)
    {
        return FormatToPeriod(new DateTimeOffset(startDate), new DateTimeOffset(endDate), cultureInfo);
    }

    /// <summary>
    ///     Converts a Unix timestamp (seconds since the Unix epoch) to a DateTime.
    /// </summary>
    /// <param name="unixTimestamp">The Unix timestamp to convert.</param>
    /// <returns>A DateTime representing the converted date and time.</returns>
    public static DateTime FromUnixTimestamp(this double unixTimestamp)
    {
        var baseBase = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp)
            .ToLocalTime();

        return baseBase;
    }

    /// <summary>
    ///     Converts a DateTime to a Unix timestamp (seconds since the Unix epoch).
    /// </summary>
    /// <param name="dateTime">The DateTime to convert.</param>
    /// <returns>A double representing the Unix timestamp.</returns>
    public static double ToUnixTimestamp(this DateTime dateTime)
    {
        return dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    /// <summary>
    ///     Converts a DateTimeOffset to a Unix timestamp (seconds since the Unix epoch).
    /// </summary>
    /// <param name="dateTime">The DateTimeOffset to convert.</param>
    /// <returns>A double representing the Unix timestamp.</returns>
    public static double ToUnixTimestamp(this DateTimeOffset dateTime)
    {
        return dateTime.UtcDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    /// <summary>
    ///     Formats a date period between two DateTimeOffset instances as a string, considering culture information.
    /// </summary>
    /// <param name="startDate">The start date of the period.</param>
    /// <param name="endDate">The end date of the period.</param>
    /// <param name="cultureInfo">Optional CultureInfo for formatting. Defaults to null.</param>
    /// <returns>A string representing the formatted date period.</returns>
    public static string FormatToPeriod(this DateTimeOffset startDate, DateTimeOffset endDate,
        CultureInfo cultureInfo = null)
    {
        var currentYear = DateTimeOffset.Now.Year;

        if (cultureInfo == null)
            cultureInfo = new CultureInfo("pt-BR");

        //same day
        if (endDate.Subtract(startDate) == TimeSpan.Zero) return startDate.ToString("d MMMM", cultureInfo);

        //day equal but time diferent
        if (startDate.Day == endDate.Day && startDate.Month == endDate.Month && startDate.Year == endDate.Year)
            return
                $"{startDate.ToString("d MMMM", cultureInfo)} {startDate.ToString("HH:mm", cultureInfo)} - {endDate.ToString("HH:mm", cultureInfo)}";

        string res;
        //same month
        if (startDate.Month == endDate.Month)
        {
            if (startDate.Year == endDate.Year)
            {
                res = $"{startDate.Day} - {endDate.Day} {endDate.ToString("MMMM", cultureInfo)}";

                if (startDate.Year != currentYear)
                    return res + $" {startDate.Year}";
            }
            else
            {
                return
                    $"{startDate.Day} {startDate.ToString("MMMM", cultureInfo)} {startDate.Year} - {endDate.Day} {endDate.ToString("MMMM", cultureInfo)} {endDate.Year}";
            }
        }
        else
        {
            if (startDate.Year == endDate.Year)
            {
                res =
                    $"{startDate.Day} {startDate.ToString("MMMM", cultureInfo)} - {endDate.Day} {endDate.ToString("MMMM", cultureInfo)}";

                if (startDate.Year != currentYear)
                    return res + $" {startDate.Year}";
            }
            else
            {
                return
                    $"{startDate.Day} {startDate.ToString("MMMM", cultureInfo)} {startDate.Year} - {endDate.Day} {endDate.ToString("MMMM", cultureInfo)} {endDate.Year}";
            }
        }

        return res;
    }

    /// <summary>
    ///     Converts one timezone to another using the offset difference.
    /// </summary>
    /// <param name="sourceDateTime">The datetime without timezone</param>
    /// <param name="sourceOffset">The offset from the source</param>
    /// <param name="targetOffset">The final offset.</param>
    /// <returns>A DateTime with the difference.</returns>
    public static DateTime ToTimeZone(this DateTime sourceDateTime, TimeSpan sourceOffset, TimeSpan targetOffset)
    {
        var offsetDifference = targetOffset - sourceOffset;

        // Adjust the source DateTime by the offset difference
        return sourceDateTime + offsetDifference;
    }

    /// <summary>
    ///     Converts one timezone to another using the offset difference.
    /// </summary>
    /// <param name="sourceDateTime">The datetime without timezone</param>
    /// <param name="targetOffset">The final offset.</param>
    /// <returns>A DateTime with the difference.</returns>
    public static DateTime ToTimeZone(this DateTimeOffset sourceDateTime, TimeSpan targetOffset)
    {
        return ToTimeZone(sourceDateTime.DateTime, sourceDateTime.Offset, targetOffset);
    }

    /// <summary>
    ///     Converts a boolean value to a "Sim" (Yes) or "Não" (No) string representation.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>"Sim" if the value is true; otherwise, "Não".</returns>
    public static string ToYesNo(this bool value)
    {
        return value ? "Sim" : "Não";
    }

    /// <summary>
    ///     Converts an integer value to a string array containing numbers from 0 to the specified value.
    /// </summary>
    /// <param name="value">The integer value specifying the upper bound of the array.</param>
    /// <returns>An array of strings containing numbers from 0 to the specified value.</returns>
    public static string[] ConvertToStringArray(this int value)
    {
        var res = new string[value + 1];

        for (var i = 0; i <= value; i++) res[i] = i.ToString();

        return res;
    }

    /// <summary>
    ///     Calculates the start of the week for a given DateTimeOffset based on the specified DayOfWeek.
    /// </summary>
    /// <param name="dt">The DateTimeOffset for which to find the start of the week.</param>
    /// <param name="startOfWeek">The DayOfWeek that defines the start of the week.</param>
    /// <returns>The DateTimeOffset representing the start of the week.</returns>
    public static DateTimeOffset StartOfWeek(this DateTimeOffset dt, DayOfWeek startOfWeek)
    {
        var diff = dt.DayOfWeek - startOfWeek;
        if (diff < 0) diff += 7;

        return dt.AddDays(-1 * diff).Date;
    }

    /// <summary>
    ///     Returns a DateTimeOffset with the same date as the input DateTimeOffset but with the time set to midnight
    ///     (00:00:00).
    /// </summary>
    /// <param name="now">The input DateTimeOffset.</param>
    /// <returns>A DateTimeOffset with the same date and midnight time.</returns>
    public static DateTimeOffset OnlyDate(this DateTimeOffset now)
    {
        return new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, 0, now.Offset);
    }

    /// <summary>
    ///     Converts a DateTime to the Brazilian time zone (UTC-3).
    /// </summary>
    /// <param name="date">The DateTime to convert.</param>
    /// <returns>The DateTime converted to the Brazilian time zone.</returns>
    public static DateTime ToBrazilianTimeZone(this DateTime date)
    {
        var dateUtc = date.ToUniversalTime().Subtract(TimeSpan.FromHours(3));

        return dateUtc;
    }

    /// <summary>
    ///     Masks the credit card number by replacing the middle digits with asterisks.
    /// </summary>
    /// <param name="number">The credit card number to mask.</param>
    /// <returns>The masked credit card number.</returns>
    public static string MaskCreditCard(this string number)
    {
        if (number.IsNullOrEmpty())
            return number;

        if (number.Length <= 10)
            return number;

        const int beginLength = 6;
        const int endLength = 4;
        var middleLength = number.Length - beginLength - endLength;

        var result = number.Substring(0, beginLength);

        result = result.PadRight(middleLength, '*');

        return result;
    }

    /// <summary>
    ///     Converts a string to a uri base 64 pattern
    /// </summary>
    /// <param name="stream">The Stream file.</param>
    /// <param name="mimeType">The stream mimeType</param>
    /// <returns></returns>
    public static string ToDataUriBase64(this Stream stream, string mimeType)
    {
        if (stream == null || string.IsNullOrWhiteSpace(mimeType))
            return null;

        using var mStream = new MemoryStream();
        stream.CopyTo(mStream);
        return "data:" + mimeType + ";base64," + Convert.ToBase64String(mStream.ToArray());
    }

    /// <summary>
    ///     Converts a stream to a base 64 string
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string ToBase64(this Stream stream)
    {
        if (stream == null)
            return null;

        using var mStream = new MemoryStream();
        stream.CopyTo(mStream);
        return Convert.ToBase64String(mStream.ToArray());
    }

    /// <summary>
    ///     Returns a mimetype based on the file extension
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetMimeType(this string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);

        return fileExtension switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }

    #region [From Net Code]

    /// <summary>
    ///     Copies data from one stream to another.
    /// </summary>
    /// <param name="src">The source stream to copy from.</param>
    /// <param name="dest">The destination stream to copy to.</param>
    public static void CopyTo(Stream src, Stream dest)
    {
        var bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
    }

    /// <summary>
    ///     Compresses a string into a byte array using GZip compression.
    /// </summary>
    /// <param name="str">The string to compress.</param>
    /// <returns>The compressed byte array.</returns>
    public static byte[] Zip(this string str)
    {
        return Zip(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    ///     Compresses a byte array using GZip compression.
    /// </summary>
    /// <param name="bytes">The byte array to compress.</param>
    /// <returns>The compressed byte array.</returns>
    public static byte[] Zip(this byte[] bytes)
    {
        using var msi = new MemoryStream(bytes);
        using var mso = new MemoryStream();
        using (var gs = new GZipStream(mso, CompressionMode.Compress))
        {
            CopyTo(msi, gs);
        }

        return mso.ToArray();
    }

    /// <summary>
    ///     Decompresses a compressed byte array using GZip compression into a string.
    /// </summary>
    /// <param name="bytes">The compressed byte array to decompress.</param>
    /// <returns>The decompressed string.</returns>
    public static string Unzip(this byte[] bytes)
    {
        using var msi = new MemoryStream(bytes);
        using var mso = new MemoryStream();
        using (var gs = new GZipStream(msi, CompressionMode.Decompress))
        {
            CopyTo(gs, mso);
        }

        return Encoding.UTF8.GetString(mso.ToArray());
    }

    #endregion
}