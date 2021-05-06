// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Documents
{
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
}