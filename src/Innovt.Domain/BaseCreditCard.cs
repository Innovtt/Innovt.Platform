using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain
{
    public class BaseCreditCard : ValueObject
    {
        public string Number { get; set; }
        public string Holder { get; set; }

        public string SecurityCode { get; set; }

        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string MaskedNumber
        {
            get
            {
                var maskedNumber = GetMaskedCreditCard(Number);

                return maskedNumber;
            }
        }

        public static string GetMaskedCreditCard(string number)
        {
            string result = "******";

            if (number != null && number.Length > 10)
            {
                string first = number.Substring(0, 6);
                string last = number.Substring(number.Length - 4, 4);

                result = first + result + last;
            }

            return result;
        }

        public BaseCreditCard(string number, string holder, DateTime expiration, string securityCode)
        {
            Number = number;
            Holder = holder;
            Expiration = expiration;
            SecurityCode = securityCode;
        }

        public BaseCreditCard(string token, string securityCode)
        {
            SecurityCode = securityCode;
            Token = token;
        }

        public BaseCreditCard()
        {
        }
    }
}