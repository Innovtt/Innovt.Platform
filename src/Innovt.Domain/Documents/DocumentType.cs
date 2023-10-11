// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Adresses;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Documents;

/// <summary>
/// Represents a document type entity.
/// </summary>
public class DocumentType : ValueObject
{
    /// <summary>
    /// Gets or sets the name of the document type.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the ID of the country associated with this document type.
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets the country associated with this document type.
    /// </summary>
    public Country Country { get; set; }

    /// <summary>
    /// Gets or sets the mask for the document type.
    /// </summary>
    public string Mask { get; set; }
}