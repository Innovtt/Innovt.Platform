using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Model.Address
{
    public class AddressType : ValueObject
    {
        protected static List<AddressType> types = new List<AddressType>();

        public static AddressType Comercial = new AddressType(1, "Comercial");
        public static AddressType Residential = new AddressType(2, "Residencial");
        public static AddressType Pagamento = new AddressType(3, "Pagamento");

        public string Name { get; set; }

        public AddressType(int id, string name)
        {
            Id = id;
            Name = name;
            types.Add(this);
        }

        public static List<AddressType> FindAll()
        {
            return types.OrderBy(c => c.Name).ToList();
        }
        public static AddressType GetByPk(int id)
        {
            return types.SingleOrDefault(s => s.Id == id);
        }
    }
}