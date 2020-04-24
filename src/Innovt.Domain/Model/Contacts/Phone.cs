// Company: INNOVT
// Project: Innovt.Domain
// Created By: Michel Borges
// Date; 18
namespace Innovt.Domain.Model.Contacts
{
    public class Phone:ValueObject
    {
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
    }
}