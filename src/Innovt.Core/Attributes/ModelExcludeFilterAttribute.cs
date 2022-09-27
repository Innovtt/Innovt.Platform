// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.CompilerServices;

namespace Innovt.Core.Attributes;

/// <summary>
///     You can use it for different purpose at the framework
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ModelExcludeFilterAttribute : Attribute
{
    public ModelExcludeFilterAttribute([CallerMemberName] string propertyName = null)
    {
        ExcludeAttributes = new[] { propertyName };
    }

    public ModelExcludeFilterAttribute(params string[] excludeAttributes)
    {
        ExcludeAttributes = excludeAttributes;
    }

    public string[] ExcludeAttributes { get; internal set; }
}