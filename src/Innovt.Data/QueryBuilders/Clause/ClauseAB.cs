using System;

namespace Innovt.Data.QueryBuilders.Clause
{   
    public abstract class ClauseAB :IClause
    {
        public string Name { get; private set; }

        protected ClauseAB(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public interface IClause
    {
         string Name { get; }
    }
}