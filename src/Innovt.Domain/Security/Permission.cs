// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security;
/// <summary>
/// Represents a permission entity.
/// </summary>
public class Permission : ValueObject<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Permission"/> class.
    /// </summary>
    public Permission()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    ///     Can be the area in your Controller
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    ///     The custom name that you need to show to your customer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     * - mean that you want to authorize the full path/domain
    ///     Controller/* mean that you can authorize all actions
    ///     Controller/Action mean that you want to authorize only this action
    /// </summary>
    public string Resource { get; set; }
}