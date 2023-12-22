// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;

namespace Innovt.Core.Test.Models;

/// <summary>
/// Represents a class 'B' with properties for Id, Role, and Document.
/// </summary>
public class B
{
    /// <summary>
    /// Gets or sets the unique identifier for an instance of class 'B'.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the role associated with an instance of class 'B'.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets the document information associated with an instance of class 'B'.
    /// </summary>
    public string Document { get; set; }
}