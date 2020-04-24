using System;
using System.Runtime.CompilerServices;

namespace Innovt.Core.Attributes
{
    /// <summary>
    /// You can use it for different purpose at the framework
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ModelExcludeFilterAttribute :  Attribute
    {
        public string[] ExcludeAttributes { get; internal set; }

        public ModelExcludeFilterAttribute([CallerMemberName] string propertyName = null)
        {
            this.ExcludeAttributes = new[] { propertyName };
        }

        public ModelExcludeFilterAttribute(params string[] excludeAttributes)
        {
            this.ExcludeAttributes = excludeAttributes;
        }
    }
}
