// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

// ReSharper disable MemberCanBePrivate.Global

namespace Innovt.Core.Utilities;

/// <summary>
///     Provides a collection of extension methods for string manipulation and validation.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Determines whether the string represents a valid email address.
    /// </summary>
    /// <param name="value">The string to validate as an email address.</param>
    /// <returns>True if the string is a valid email address; otherwise, false.</returns>
    public static bool IsEmail(this string value)
    {
        if (value.IsNullOrEmpty())
            return false;

        var isValid = new EmailAddressAttribute().IsValid(value);

        return isValid;
    }

    /// <summary>
    ///     Determines whether the string consists of numeric characters only.
    /// </summary>
    /// <param name="value">The string to validate as a number.</param>
    /// <returns>True if the string consists of numeric characters only; otherwise, false.</returns>
    public static bool IsNumber(this string value)
    {
        return value != null && value.All(char.IsNumber);
    }

    /// <summary>
    ///     Truncates the string to a specified maximum length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the truncated string.</param>
    /// <returns>
    ///     The truncated string, or the original string if its length is less than or equal to the specified maximum
    ///     length.
    /// </returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;

        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    ///     Determines whether the string is a valid CPF (Cadastro de Pessoas Físicas) number.
    /// </summary>
    /// <param name="value">The string to validate as a CPF number.</param>
    /// <returns>True if the string is a valid CPF number; otherwise, false.</returns>
    public static bool IsCpf(this string value)
    {
        if (value.IsNullOrEmpty())
            return false;

        var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string corpovalue;
        string digitoVerificador;
        int soma;
        int resto;

        value = value.OnlyNumber();

        if ((value.Length != 11) | value.Equals("11111111111", StringComparison.Ordinal) |
            value.Equals("22222222222", StringComparison.Ordinal) |
            value.Equals("33333333333", StringComparison.Ordinal) |
            value.Equals("44444444444", StringComparison.Ordinal) |
            value.Equals("55555555555", StringComparison.Ordinal) |
            value.Equals("66666666666", StringComparison.Ordinal) |
            value.Equals("77777777777", StringComparison.Ordinal) |
            value.Equals("88888888888", StringComparison.Ordinal) |
            value.Equals("99999999999", StringComparison.Ordinal))
            return false;

        corpovalue = value.Substring(0, 9);
        soma = 0;
        for (var i = 0; i < 9; i++)
            soma += int.Parse(corpovalue[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digitoVerificador = resto.ToString();

        corpovalue += digitoVerificador;

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += int.Parse(corpovalue[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digitoVerificador += resto.ToString();

        return value.EndsWith(digitoVerificador);
    }

    //Codigo referencia http://www.macoratti.net/11/09/c_val1.htm
    /// <summary>
    ///     Determines whether the string is a valid CNPJ (Cadastro Nacional da Pessoa Jurídica) number.
    /// </summary>
    /// <param name="cnpj">The string to validate as a CNPJ number.</param>
    /// <returns>True if the string is a valid CNPJ number; otherwise, false.</returns>
    public static bool IsCnpj(this string cnpj)
    {
        if (cnpj.IsNullOrEmpty())
            return false;

        var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        cnpj = cnpj.Trim();
        cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
        if (cnpj.Length != 14)
            return false;
        var tempCnpj = cnpj.Substring(0, 12);

        var soma = 0;
        for (var i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        var resto = soma % 11;

        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        var digit = resto.ToString();

        tempCnpj += digit;
        soma = 0;
        for (var i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digit += resto;
        return cnpj.EndsWith(digit);
    }

    /// <summary>
    ///     Determines whether a Guid is not empty (contains non-zero values).
    /// </summary>
    /// <param name="yourGuid">The Guid to check.</param>
    /// <returns>True if the Guid is not empty; otherwise, false.</returns>
    public static bool IsNotEmpty(this Guid yourGuid)
    {
        return !IsEmpty(yourGuid);
    }
    
    /// <summary>
    ///     Determines whether a Guid is empty (all zeros).
    /// </summary>
    /// <param name="yourGuid">The Guid to check.</param>
    /// <returns>True if the Guid is empty; otherwise, false.</returns>
    public static bool IsEmpty(this Guid yourGuid)
    {
        return yourGuid == Guid.Empty;
    }
    
    /// <summary>
    ///     Determines whether a nullable Guid is not null or empty.
    /// </summary>
    /// <param name="yourGuid">The nullable Guid to check.</param>
    /// <returns>True if the nullable Guid is not null or empty; otherwise, false.</returns>
    public static bool IsNotNullOrEmpty([NotNullWhen(false)]this Guid? yourGuid)
    {
        return !IsNullOrEmpty(yourGuid);
    }
    
    /// <summary>
    ///     Determines whether a nullable Guid is null or empty.
    /// </summary>
    /// <param name="yourGuid">The nullable Guid to check.</param>
    /// <returns>True if the nullable Guid is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)]this Guid? yourGuid)
    {
        return yourGuid is null || IsEmpty(yourGuid.GetValueOrDefault());
    }

    /// <summary>
    ///  Determines whether a string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>True if the string is null, empty, or consists only of white-space characters; otherwise, false.</returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
    /// <summary>
    ///     Determines whether a string is not null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>
    ///     True if the string is not null, not empty, and not consisting only of white-space characters; otherwise,
    ///     false.
    /// </returns>
    public static bool IsNotNullOrEmpty([NotNullWhen(false)]this string str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }
    
    /// <summary>
    ///     Encodes a string for safe use in a URL, using UTF-8 encoding.
    /// </summary>
    /// <param name="str">The string to encode.</param>
    /// <returns>The URL-encoded string, or the original string if it is null or empty.</returns>
    public static string UrlEncode(this string str)
    {
        return str.IsNullOrEmpty() ? str : HttpUtility.UrlEncode(str, Encoding.UTF8);
    }

    /// <summary>
    ///     Decodes a URL-encoded string using UTF-8 encoding.
    /// </summary>
    /// <param name="str">The URL-encoded string to decode.</param>
    /// <returns>The decoded string, or the original string if it is null or empty.</returns>
    public static string UrlDecode(this string str)
    {
        return str.IsNullOrEmpty() ? str : HttpUtility.UrlDecode(str, Encoding.UTF8);
    }

    /// <summary>
    ///     Gets the value of a string or a default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>
    ///     The original string if it is not null or empty; otherwise, the default value if provided; otherwise, an empty
    ///     string.
    /// </returns>
    public static string GetValueOrDefault(this string str, string defaultValue = null)
    {
        if (!str.IsNullOrEmpty())
            return str;

        return defaultValue.IsNotNullOrEmpty() ? defaultValue : string.Empty;
    }

    /// <summary>
    ///     Trims leading and trailing white-space characters from a string.
    /// </summary>
    /// <param name="str">The string to trim.</param>
    /// <returns>The trimmed string or null if the input string is null.</returns>
    public static string SmartTrim(this string str)
    {
        return str?.Trim();
    }

    /// <summary>
    ///     Will mask an email using this format mich***@gmail.com
    /// </summary>
    /// <param name="email">The email that you want to mask</param>
    /// <returns>mich***@gmail.com</returns>
    public static string MaskEmail(this string email)
    {
        if (email.IsNullOrEmpty())
            return email;

        if (!email.Contains("@"))
            return new string('*', email.Length);

        if (email.Split('@')[0].Length < 4)
            return @"*@*.*";

        var pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";

        var result = Regex.Replace(email, pattern, m => new string('*', m.Length));

        return result;
    }

   

    /// <summary>
    ///     Converts a delimited string into a list of strings using the specified separator character.
    /// </summary>
    /// <param name="str">The delimited string to convert.</param>
    /// <param name="separator">The character used to separate values in the string.</param>
    /// <returns>A list of strings containing the individual values from the delimited string.</returns>
    public static List<string> ToList(this string str, char separator)
    {
        return str.IsNullOrEmpty() ? [] : str.Split(separator).ToList();
    }

    /// <summary>
    ///     Converts a string to camel case by applying title case.
    /// </summary>
    /// <param name="str">The string to convert to camel case.</param>
    /// <returns>The string in camel case format.</returns>
    public static string ToCamelCase(this string str)
    {
        return str.IsNullOrEmpty() ? string.Empty : str.ToLowerInvariant().ToTitleCase();
    }

    /// <summary>
    ///     Converts a string to title case using the current culture's rules.
    /// </summary>
    /// <param name="str">The string to convert to title case.</param>
    /// <returns>The string in title case format.</returns>
    public static string ToTitleCase(this string str)
    {
        return str.IsNullOrEmpty() ? string.Empty : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
    }

    /// <summary>
    ///     Removes specified special characters from a string.
    /// </summary>
    /// <param name="value">The string to remove special characters from.</param>
    /// <returns>The string with specified special characters removed.</returns>
    public static string ClearMask(this string value)
    {
        if (value.IsNullOrEmpty())
            return value;

        var specialChars = new[]
            { ".", ",", "-", "_", "/", "\\", "(", ")", "[", "]", ":", "\r\n", "\r", "\n" };

        for (var i = 0; i < specialChars.Length; i++)
            value = value.Replace(specialChars[i], "", StringComparison.OrdinalIgnoreCase);

        return value;
    }

    /// <summary>
    ///     Removes all non-numeric characters from a string.
    /// </summary>
    /// <param name="value">The string to remove non-numeric characters from.</param>
    /// <returns>The string containing only numeric characters.</returns>
    public static string OnlyNumber(this string value)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;

        return Regex.Replace(value, "[^0-9]+?", "");
    }

    /// <summary>
    ///     Normalizes a string by removing accents, special characters, and trimming whitespace.
    /// </summary>
    /// <param name="str">The string to normalize.</param>
    /// <returns>The normalized string.</returns>
    public static string NormalizeText(this string str)
    {
        if (str.IsNullOrEmpty())
            return string.Empty;

        var result = str.RemoveAccents().RemoveSpecialCharacter().TrimStart().TrimEnd();

        return result;
    }

    /// <summary>
    ///     Removes all special characters from a string.
    /// </summary>
    /// <param name="str">The string to remove special characters from.</param>
    /// <returns>The string with special characters replaced by spaces.</returns>
    public static string RemoveSpecialCharacter(this string str)
    {
        if (str.IsNullOrEmpty())
            return string.Empty;

        return Regex.Replace(str, "[^0-9a-zA-Z]+", " ");
    }

    /// <summary>
    ///     Removes accents from a string.
    /// </summary>
    /// <param name="str">The string to remove accents from.</param>
    /// <returns>The string with accents removed.</returns>
    public static string RemoveAccents(this string str)
    {
        if (str.IsNullOrEmpty())
            return string.Empty;

        //code from Web: https://www.codegrepper.com/code-examples/csharp/c%23+remove+accents
        var sbReturn = new StringBuilder();
        var arrayText = str.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (var letter in arrayText)
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);

        return sbReturn.ToString();
    }

    /// <summary>
    ///     Formats a string as a CPF (Cadastro de Pessoas Físicas) number with a mask (e.g., "000.000.000-00").
    /// </summary>
    /// <param name="cpf">The string to format as a CPF.</param>
    /// <returns>The formatted CPF string or the original string if it is null or empty.</returns>
    public static string FormatCpf(this string cpf)
    {
        if (cpf.IsNullOrEmpty())
            return string.Empty;
        
        cpf = cpf.PadLeft(11, '0');
        return FormatByMask(cpf, @"{0:000\.000\.000\-00}");
    }

    /// <summary>
    ///     You can use this method to forma DD stardand Latitude or Longitude
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatCoordinate(this long value)
    {
        var multiplier = value > 0 ? "" : "-";

        var latitude = value.ToString().OnlyNumber().PadLeft(9, '0');

        return $"{multiplier}{latitude.Substring(0, 2)}.{latitude.Substring(2, 7)}";
    }

    /// <summary>
    ///     Formats a string as a cell phone number with a mask (e.g., "(00)00000-0000").
    /// </summary>
    /// <param name="celPhone">The string to format as a cell phone number.</param>
    /// <returns>The formatted cell phone number string or the original string if it is null or empty.</returns>
    public static string FormatCelPhone(this string celPhone)
    {
        return FormatByMask(celPhone, @"{0:\(00\)00000\-0000}");
    }

    /// <summary>
    ///     Formats a string as a phone number with a mask (e.g., "(00)000-0000").
    /// </summary>
    /// <param name="phoneNumber">The string to format as a phone number.</param>
    /// <returns>The formatted phone number string or the original string if it is null or empty.</returns>
    public static string FormatPhoneNumber(this string phoneNumber)
    {
        return FormatByMask(phoneNumber, @"{0:\(00\)000\-0000}");
    }

    /// <summary>
    ///     Formats a string as a CNPJ (Cadastro Nacional da Pessoa Jurídica) number with a mask (e.g., "00.000.000/0000-00").
    /// </summary>
    /// <param name="cnpj">The string to format as a CNPJ number.</param>
    /// <returns>The formatted CNPJ string or null if the input is null or empty.</returns>
    public static string FormatCnpj(this string cnpj)
    {
        if (cnpj.IsNullOrEmpty())
            return string.Empty;

        cnpj = cnpj.PadLeft(14, '0');

        return FormatByMask(cnpj, @"{0:00\.000\.000\/0000\-00}");
    }

    /// <summary>
    ///     Converts the first letter of a string to uppercase.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string with its first letter in uppercase or the original string if it is null or empty.</returns>
    public static string ToUpperFirstLetter(this string value)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;

        var firstLetter = value[0].ToString().ToUpper();
        var tail = value[1..];

        return firstLetter + tail;
    }

    /// <summary>
    ///     Formats a string as a zip code with a mask (e.g., "00000-000").
    /// </summary>
    /// <param name="cep">The string to format as a zip code.</param>
    /// <returns>The formatted zip code string or the original string if it is null or empty.</returns>
    public static string FormatZipCode(this string cep)
    {
        return FormatByMask(cep, @"{0:00000\-000}");
    }

    /// <summary>
    ///     Formats a string using a specified mask by extracting numeric characters from the input string.
    /// </summary>
    /// <param name="value">The string to format.</param>
    /// <param name="mascara">The mask to apply.</param>
    /// <returns>The formatted string.</returns>
    private static string FormatByMask(string value, string mascara)
    {
        var numbers = value.OnlyNumber();

        var lgValue = long.Parse(numbers);

        return string.Format(mascara, lgValue);
    }


    /// <summary>
    ///     Encode your string to Base64
    /// </summary>
    /// <param name="toEncode">The String to encode</param>
    /// <param name="encoding">Optional and will be ASCII if null</param>
    /// <returns></returns>
    public static string ToBase64(this string toEncode, Encoding encoding = null)
    {
        if (toEncode == null)
            return string.Empty;

        encoding ??= Encoding.ASCII;

        var btToEncode = encoding.GetBytes(toEncode);

        return Convert.ToBase64String(btToEncode);
    }

    /// <summary>
    ///     Decode  your string from Base64
    /// </summary>
    /// <param name="toDecode">The string to decode</param>
    /// <param name="encoding">Optional and will be ASCII if null</param>
    /// <returns></returns>
    public static string FromBase64(this string toDecode, Encoding encoding = null)
    {
        if (toDecode == null)
            return string.Empty;

        encoding ??= Encoding.ASCII;

        var btToDecode = Convert.FromBase64String(toDecode);

        return encoding.GetString(btToDecode);
    }

    /// <summary>
    ///     Encloses a string in double quotes if it contains spaces.
    /// </summary>
    /// <param name="value">The string to enclose in double quotes.</param>
    /// <returns>The original string if it does not contain spaces; otherwise, the string enclosed in double quotes.</returns>
    public static string ApplyQuotes(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (!value.Contains(" "))
            return value;

        return "\"" + value + "\"";
    }

    /// <summary>
    ///     Converts a string to an integer, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to an integer.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The integer value of the string, or the default value if the string is null or empty.</returns>
    public static int ToInt(this string str, int defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        return int.Parse(str);
    }

    /// <summary>
    ///     Converts a string to a nullable integer, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a nullable integer.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The nullable integer value of the string, or the default value if the string is null or empty.</returns>
    public static int? ToInt(this string str, int? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (int.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a decimal, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a decimal.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The decimal value of the string, or the default value if the string is null or empty.</returns>
    public static decimal ToDecimal(this string str, decimal defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        return decimal.Parse(str);
    }

    /// <summary>
    ///     Converts a string to a nullable decimal, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a nullable decimal.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The nullable decimal value of the string, or the default value if the string is null or empty.</returns>
    public static decimal? ToDecimal(this string str, decimal? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (decimal.TryParse(str, out var value))
            return value;


        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a double, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a double.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The double value of the string, or the default value if the string is null or empty.</returns>
    public static double ToDouble(this string str, double defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (double.TryParse(str, out var value))
            return value;


        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a nullable double, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a nullable double.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The nullable double value of the string, or the default value if the string is null or empty.</returns>
    public static double? ToDouble(this string str, double? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (double.TryParse(str, out var value))
            return value;


        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a float, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a float.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The float value of the string, or the default value if the string is null or empty.</returns>
    public static float ToFloat(this string str, float defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (float.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a nullable float, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a nullable float.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The nullable float value of the string, or the default value if the string is null or empty.</returns>
    public static float? ToFloat(this string str, float? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (float.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a boolean, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a boolean.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The boolean value of the string, or the default value if the string is null or empty.</returns>
    public static bool ToBoolean(this string str, bool defaultValue = false)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (bool.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a boolean value, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a boolean.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>
    ///     The boolean value of the string if it represents a valid boolean; otherwise, the default value.
    /// </returns>
    public static bool? ToBoolean(this string str, bool? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (bool.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a DateTime, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a DateTime.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The DateTime value of the string, or the default value if the string is null or empty.</returns>
    public static DateTime ToDateTime(this string str, DateTime defaultValue = new())
    {
        if (string.IsNullOrEmpty(str))
            return DateTime.MinValue;

        if (DateTime.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a nullable DateTime, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a nullable DateTime.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The nullable DateTime value of the string, or the default value if the string is null or empty.</returns>
    public static DateTime? ToDateTime(this string str, DateTime? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (DateTime.TryParse(str, out var value))
            return value;

        return defaultValue;
    }


    /// <summary>
    ///     Converts a string to a DateTimeOffset, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a DateTimeOffset.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The DateTimeOffset value of the string, or the default value if the string is null or empty.</returns>
    public static DateTimeOffset ToDateTimeOffset(this string str,
        DateTimeOffset defaultValue = new())
    {
        if (string.IsNullOrEmpty(str))
            return DateTimeOffset.MinValue;

        if (DateTimeOffset.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a nullable DateTimeOffset, using a specified default value if the string is null or empty.
    /// </summary>
    /// <param name="str">The string to convert to a nullable DateTimeOffset.</param>
    /// <param name="defaultValue">The default value to return if the string is null or empty (optional).</param>
    /// <returns>The nullable DateTimeOffset value of the string, or the default value if the string is null or empty.</returns>
    public static DateTimeOffset? ToDateTimeOffset(this string str, DateTimeOffset? defaultValue = null)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (DateTimeOffset.TryParse(str, out var value))
            return value;

        return defaultValue;
    }

    /// <summary>
    ///     Converts a string to a Guid.
    /// </summary>
    /// <param name="str">The string to convert to a Guid.</param>
    /// <returns>The Guid value of the string.</returns>
    public static Guid ToGuid(this string str)
    {
        return Guid.Parse(str);
    }

    /// <summary>
    ///     Converts a string to a Guid or returns Guid.Empty if the conversion fails.
    /// </summary>
    /// <param name="str">The string to convert to a Guid.</param>
    /// <returns>The Guid value of the string or Guid.Empty if the conversion fails.</returns>
    public static Guid? ToGuidOrEmpty(this string str)
    {
        return Guid.TryParse(str, out var result) ? result : Guid.Empty;
    }
}