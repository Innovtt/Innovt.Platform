// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Collections.Generic;
using System.Linq;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

/// <summary>
///     Represents the type of an address (e.g., Commercial, Residential, Payment).
/// </summary>
public sealed class AddressType : ValueObject
{
    private static readonly List<AddressType> Types = new();

    /// <summary>
    ///     Gets the predefined address type for commercial addresses.
    /// </summary>
    public static readonly AddressType Comercial = new(1, "Comercial");

    /// <summary>
    ///     Gets the predefined address type for residential addresses.
    /// </summary>
    public static readonly AddressType Residential = new(2, "Residencial");

    /// <summary>
    ///     Gets the predefined address type for payment-related addresses.
    /// </summary>
    public static readonly AddressType Pagamento = new(3, "Pagamento");

    /// <summary>
    ///     Initializes a new instance of the <see cref="AddressType" /> class.
    /// </summary>
    /// <param name="id">The unique identifier for the address type.</param>
    /// <param name="name">The name of the address type.</param>
    public AddressType(int id, string name)
    {
        Id = id;
        Name = name;
        Types.Add(this);
    }

    /// <summary>
    ///     Gets or sets the name of the address type.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Retrieves a list of all available address types, ordered by name.
    /// </summary>
    /// <returns>A list of address types.</returns>
    public static IList<AddressType> FindAll()
    {
        return Types.OrderBy(c => c.Name).ToList();
    }

    /// <summary>
    ///     Retrieves an address type based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the address type.</param>
    /// <returns>An address type.</returns>
    public static AddressType GetByPk(int id)
    {
        return Types.SingleOrDefault(s => s.Id == id);
    }
}