// Company: INNOVT
// Project: Innovt.Domain
// Created By: Michel Borges
// Date; 18

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Contacts
{
    public class Phone:ValueObject
    {
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
    }
}