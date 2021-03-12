using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Innovt.Core.Utilities
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string value)
        {
            var isValid = new EmailAddressAttribute().IsValid(value);

            return isValid;
        }

        public static bool IsNumber(this string value)
        {
            if (value == null)
                return false;

            return value.All(char.IsNumber);
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        
        public static bool IsCpf(this string value)
        {
            if (value.IsNullOrEmpty())
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string corpovalue;
            string digitoVerificador;
            int soma;
            int resto;

            value = value.OnlyNumber();

            if ((value.Length != 11) | (value.Equals("11111111111")) | (value.Equals("22222222222")) |
                (value.Equals("33333333333")) | (value.Equals("44444444444")) | (value.Equals("55555555555")) |
                (value.Equals("66666666666")) | (value.Equals("77777777777")) | (value.Equals("88888888888")) |
                (value.Equals("99999999999")))
                return false;

            corpovalue = value.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += Int32.Parse(corpovalue[i].ToString()) * multiplicador1[i];

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

        public static bool IsCnpj(this string cnpj)
        {
            if (cnpj.IsNullOrEmpty())
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            var tempCnpj = cnpj.Substring(0, 12);
          
            var soma = 0;
            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            var resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            var digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }
        
        public static bool IsNotEmpty(this Guid guid)
        {
            return !IsEmpty(guid);
        }
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return IsEmpty(guid.GetValueOrDefault());
        }
        
        public static bool IsNotNullOrEmpty(this Guid? guid)
        {
            return !IsNullOrEmpty(guid);
        }
        
        public static string UrlEncode(this string str)
        {
            return str.IsNullOrEmpty() ? str : HttpUtility.UrlEncode(str, Encoding.UTF8);
        }
        
        public static string UrlDecode(this string str)
        {   
            return str.IsNullOrEmpty() ? str : HttpUtility.UrlDecode(str, Encoding.UTF8);
        }

        public static string GetValueOrDefault(this string str, string defaultValue = null)
        {
            if (!str.IsNullOrEmpty())
                return str;


            if (defaultValue.IsNotNullOrEmpty())
                return defaultValue;

            return string.Empty;
        }

        public static string SmartTrim(this string str)
        {
            return str?.Trim();
        }

        /// <summary>
        /// Will mask an email using this format mich***@gmail.com
        /// </summary>
        /// <param name="email">The email that you want to mask</param>
        /// <returns>mich***@gmail.com</returns>
        public static string MaskEmail(this string email)
        {
            if (email.IsNullOrEmpty())
                return email;

            if (!email.Contains("@"))
                return new String('*', email.Length);

            if (email.Split('@')[0].Length < 4)
                return @"*@*.*";

            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";

            string result = Regex.Replace(email, pattern, m => new string('*', m.Length));

            return result;
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !String.IsNullOrWhiteSpace(str);
        }

        public static List<string> ToList(this string str, char separator)
        {
            if (str.IsNullOrEmpty())
                return new List<string>();

            return str.Split(separator).ToList();
        }
        
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        public static string ToCamelCase(this string str) => str.ToTitleCase();
      

        public static string ClearMask(this string value)
        {
            if (value.IsNullOrEmpty())
                return value;

            var specialChars = new string[] { ".", ",", "-", "_", "/", "\\", "(", ")", "[", "]", ":", "\r\n", "\r", "\n" };

            for (var i = 0; i < specialChars.Length; i++)
            {
                value = value.Replace(specialChars[i], "");
            }
            return value;
        }

        public static string OnlyNumber(this string value)
        {
            if (value.IsNullOrEmpty())
                return value;

            return Regex.Replace(value, "[^0-9]+?", "");
        }
        
        
        public static string NormalizeText(this string str)
        {
            if (str.IsNullOrEmpty())
                return str;

            var result = str.RemoveAccents().TrimStart().TrimEnd();

            return result;
        }

        public static string RemoveAccents(this string str)
        {
            if (str.IsNullOrEmpty())
                return str;

            byte[] bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(str);
            return Encoding.UTF8.GetString(bytes);
        }


        public static string FormatCPF(this string cpf)
        {
            if (cpf.IsNullOrEmpty())
                return cpf;

            cpf = cpf.PadLeft(11, '0');
            return FormatByMask(cpf, @"{0:000\.000\.000\-00}");
        }

        /// <summary>
        /// You can use this method to forma DD stardand Latitude or Longitude
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatCoordinate(this long value)
        {
           var multiplier = value>0 ? "" : "-";

           var latitude = value.ToString().OnlyNumber().PadLeft(9,'0');

           return $"{multiplier}{latitude.Substring(0,2)}.{latitude.Substring(2, 7)}";
        }


        public static string FormatCelPhone(this string celPhone)
        {
            return FormatByMask(celPhone, @"{0:\(00\)00000\-0000}");
        }
        public static string FormatPhoneNumber(this string phoneNumber)
        {
            return FormatByMask(phoneNumber, @"{0:\(00\)000\-0000}");
        }

        public static string FormatCNPJ(this string cnpj)
        {
            cnpj = cnpj.PadLeft(14, '0');

            return FormatByMask(cnpj, @"{0:00\.000\.000\/0000\-00}");
        }

        public static string ToUpperFirstLetter(this string value)
        {
            if (value.IsNullOrEmpty())
                return value;

            var firstLetter = value[0].ToString().ToUpper();
            var tail = value.Substring(1, value.Length - 1);

            return firstLetter + tail;
        }


        public static string FormatZipCode(this string cep)
        {
            return FormatByMask(cep, @"{0:00000\-000}");
        }

        private static string FormatByMask(string value, string mascara)
        {
            var numbers = value.OnlyNumber();

            long lgValue = Int64.Parse(numbers);

            return String.Format(mascara, lgValue);
        }


        /// <summary>
        /// Encode your string to Base64 
        /// </summary>
        /// <param name="toEncode">The String to encode</param>
        /// <param name="encoding">Optional and will be ASCII if null</param>
        /// <returns></returns>
        public static string ToBase64(this string toEncode, Encoding encoding = null)
        {
            if (toEncode == null)
                return toEncode;

            if (encoding == null)
                encoding = Encoding.ASCII;

            var btToEncode = encoding.GetBytes(toEncode);

            return Convert.ToBase64String(btToEncode);
        }
        /// <summary>
        /// Decode  your string from Base64 
        /// </summary>
        /// <param name="toDecode">The string to decode</param>
        /// <param name="encoding">Optional and will be ASCII if null</param>
        /// <returns></returns>
        public static string FromBase64(this string toDecode, Encoding encoding = null)
        {
            if (toDecode == null)
                return toDecode;

            if (encoding == null)
                encoding = Encoding.ASCII;

            var btToDecode = Convert.FromBase64String(toDecode);

            return encoding.GetString(btToDecode);
        }


        public static string AplyQuotes(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (!value.Contains(" "))
                return value;

            return "\"" + value + "\"";
        }

        public static int ToInt(this string str, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            return int.Parse(str);
        }

        public static int? ToInt(this string str, int? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (int.TryParse(str, out int value))
                return value;


            return defaultValue;
        }


        public static decimal ToDecimal(this string str, decimal defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            return decimal.Parse(str);
        }

        public static decimal? ToDecimal(this string str, decimal? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (decimal.TryParse(str, out decimal value))
                return value;


            return defaultValue;
        }

        public static double ToDouble(this string str, double defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (double.TryParse(str, out double value))
                return value;


            return defaultValue;
        }

        public static double? ToDouble(this string str, double? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (double.TryParse(str, out double value))
                return value;


            return defaultValue;
        }

        public static float ToFloat(this string str, float defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (float.TryParse(str, out float value))
                return value;

            return defaultValue;
        }

        public static float? ToFloat(this string str, float? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (float.TryParse(str, out float value))
                return value;

            return defaultValue;
        }


        public static bool ToBoolean(this string str, bool defaultValue = false)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (bool.TryParse(str, out bool value))
                return value;

            return defaultValue;
        }

        public static bool? ToBoolean(this string str, bool? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (bool.TryParse(str, out bool value))
                return value;

            return defaultValue;
        }

        public static DateTime ToDateTime(this string str, DateTime defaultValue = new DateTime())
        {
            if (string.IsNullOrEmpty(str))
                return DateTime.MinValue;

            if (DateTime.TryParse(str, out DateTime value))
                return value;

            return defaultValue;
        }

        public static DateTime? ToDateTime(this string str, DateTime? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (DateTime.TryParse(str, out DateTime value))
                return value;

            return defaultValue;
        }


        public static DateTimeOffset ToDateTimeOffset(this string str, DateTimeOffset defaultValue = new DateTimeOffset())
        {
            if (string.IsNullOrEmpty(str))
                return DateTimeOffset.MinValue;

            if (DateTimeOffset.TryParse(str, out DateTimeOffset value))
                return value;

            return defaultValue;
        }

        public static DateTimeOffset? ToDateTimeOffset(this string str, DateTimeOffset? defaultValue = null)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (DateTimeOffset.TryParse(str, out DateTimeOffset value))
                return value;

            return defaultValue;
        }

    }
}
