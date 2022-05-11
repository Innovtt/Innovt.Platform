// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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