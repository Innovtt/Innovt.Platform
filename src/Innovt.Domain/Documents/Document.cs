﻿

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Documents
{
    public class Document : ValueObject
    {   
        public int DocumentTypeId { get; set; }

        public DocumentType DocumentType { get; set; }

        public string Number { get; set; }

        public Document()
        {
            
        }

        public Document(string number,int documentTypeId)
        {
            DocumentTypeId = documentTypeId;
            Number = number;
        }
    }
}
