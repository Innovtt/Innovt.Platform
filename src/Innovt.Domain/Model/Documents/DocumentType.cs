using Innovt.Domain.Model.Address;

namespace Innovt.Domain.Model.Documents
{
    public class DocumentType : ValueObject
    {
        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public string Mask { get; set; }
    }
}
