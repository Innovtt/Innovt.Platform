// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Users
{
    public class Gender : DomainModel<Gender>
    {
        public static Gender Male = new Gender(1, 'M', "Masculino");
        public static Gender Female = new Gender(2, 'F', "Feminino");

        public Gender()
        {
        }

        public Gender(int id, char acronym, string description)
        {
            Id = id;
            Acronym = acronym;
            Description = description;
            AddModel(this);
        }

        public char Acronym { get; set; }

        public string Description { get; set; }
    }
}