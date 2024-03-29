﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Runtime.CompilerServices;

namespace Innovt.Core.Attributes;

/// <summary>
///     You can use it for different purpose at the framework
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
/// <summary>
/// Attribute used to specify model properties that should be excluded when applying filters.
/// </summary>
/// <remarks>
/// This attribute allows developers to annotate model properties that should be excluded when applying filters
/// to the model. Filters can be used, for example, for data validation or data transformation.
/// </remarks>
public sealed class ModelExcludeFilterAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ModelExcludeFilterAttribute" /> class.
    /// </summary>
    /// <param name="propertyName">The name of the property to be excluded (optional).</param>
    public ModelExcludeFilterAttribute([CallerMemberName] string propertyName = null)
    {
        ExcludeAttributes = new[] { propertyName };
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ModelExcludeFilterAttribute" /> class.
    /// </summary>
    /// <param name="excludeAttributes">An array of property names to be excluded.</param>
    public ModelExcludeFilterAttribute(params string[] excludeAttributes)
    {
        ExcludeAttributes = excludeAttributes;
    }

    /// <summary>
    ///     Gets or sets an array of property names to be excluded when applying filters.
    /// </summary>
    public string[] ExcludeAttributes { get; internal set; }
}