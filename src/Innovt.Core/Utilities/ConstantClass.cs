using System;
using System.Collections.Generic;

namespace Innovt.Core.Utilities
{
    public class ConstantClass
    {
        protected ConstantClass(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(object obj)
        {
            return obj?.ToString() == Value;
        }
        public virtual bool Equals(ConstantClass obj)
        {
            return obj?.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        protected virtual bool Equals(string value)
        {
            return Value == value;
        }

        public static bool operator ==(ConstantClass a, ConstantClass b)
        {
            return a?.Value == b?.Value;
        }
        public static bool operator ==(ConstantClass a, string b)
        {
            return a?.Value == b;
        }
        public static bool operator ==(string a, ConstantClass b)
        {
            return a == b?.Value;
        }
        public static bool operator !=(ConstantClass a, ConstantClass b)
        {
            return a?.Value != b?.Value;
        }
        public static bool operator !=(ConstantClass a, string b)
        {
            return a?.Value != b;
        }
        public static bool operator !=(string a, ConstantClass b)
        {
            return a != b?.Value;
        }

        public static implicit operator string(ConstantClass value)
        {
            return value?.Value;
        }
    }
}
