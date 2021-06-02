// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Repository
{
    public class Include
    {
        public Include()
        {
            Includes = new List<string>();
        }

        public Include(params string[] includes) : this()
        {
            Includes.AddRange(includes);
        }

        public List<string> Includes { get; }

        public bool IsEmpty()
        {
            return Includes == null || !Includes.Any();
        }

        public Include Add(string param)
        {
            Includes.Add(param);
            return this;
        }

        public Include Add(params string[] parameters)
        {
            Includes.AddRange(parameters);
            return this;
        }

        public static Include New(params string[] parameters)
        {
            return new(parameters);
        }
    }
}