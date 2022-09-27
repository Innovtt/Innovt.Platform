// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Documents;

public class Document : ValueObject
{
    public Document()
    {
    }

    public Document(string number, int documentTypeId)
    {
        DocumentTypeId = documentTypeId;
        Number = number;
    }

    public int DocumentTypeId { get; set; }

    public DocumentType DocumentType { get; set; }

    public string Number { get; set; }
}