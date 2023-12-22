// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Documents;

/// <summary>
/// Represents a document entity.
/// </summary>
public class Document : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Document"/> class.
    /// </summary>
    public Document()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Document"/> class with the specified number and document type.
    /// </summary>
    /// <param name="number">The document number.</param>
    /// <param name="documentTypeId">The ID of the associated document type.</param>
    public Document(string number, int documentTypeId)
    {
        DocumentTypeId = documentTypeId;
        Number = number;
    }

    /// <summary>
    /// Gets or sets the ID of the document type associated with this document.
    /// </summary>
    public int DocumentTypeId { get; set; }

    /// <summary>
    /// Gets or sets the document type associated with this document.
    /// </summary>
    public DocumentType DocumentType { get; set; }

    /// <summary>
    /// Gets or sets the document number.
    /// </summary>
    public string Number { get; set; }
}