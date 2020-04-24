using System.Collections.Generic;
using System.Linq;


namespace Innovt.Domain.Model
{
    public class DomainModel<T>: ValueObject where T: ValueObject
    {
        private static readonly List<T> statusModel = new List<T>();

        protected void AddModel(T model)
        {
            statusModel.Add(model);
        } 
       
        public static List<T> FindAll()
        {
            return statusModel;
        }

        public static T GetByPk(int id)
        {
            return statusModel.SingleOrDefault(s => s.Id == id);
        }
    }
}
