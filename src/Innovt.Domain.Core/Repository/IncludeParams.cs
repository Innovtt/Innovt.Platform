using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Repository
{
    public class Include
    {
        public List<string> Includes { get; private set; }

        public Include()
        {
            Includes = new List<string>();
        }

        public Include(params string[] includes) : this()
        {
            Includes.AddRange(includes);
        }

        public bool IsEmpty()
        {
            return Includes == null || !Includes.Any();
        }

        public Include Add(string param)
        {
            this.Includes.Add(param);
            return this;
        }

        public Include Add(params string[] parameters)
        {
            this.Includes.AddRange(parameters);
            return this;
        }

        public static Include New(params string[] parameters)
        {
            return new Include(parameters);
        }
    }
}