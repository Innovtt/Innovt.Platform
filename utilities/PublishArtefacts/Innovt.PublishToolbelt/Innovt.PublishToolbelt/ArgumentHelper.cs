using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.PublishToolbelt
{
    public class Argument
    {
        public Argument(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }

    public static class ArgumentHelper
    {
        public static List<Argument> Parse(string[] args)
        {
            var result = new List<Argument>();


            foreach (var arg in args)
            {
                var splited = arg.Split('=');

                if (splited.Length > 1)
                {
                    result.Add(new Argument(splited[0].Trim(), splited[1].Trim()));
                }
            }

            return result;
        }

        public static string Find(string name, List<Argument> arguments)
        {
            var first = arguments.FirstOrDefault(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            return first?.Value;
        }

    }
}