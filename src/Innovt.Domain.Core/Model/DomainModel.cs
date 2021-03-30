using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Model
{
    public class DomainModel<T> : ValueObject where T : ValueObject
    {
        private readonly List<T> models = new List<T>();

        protected void AddModel(T model)
        {
            models.Add(model);
        }

        public List<T> FindAll()
        {
            return models;
        }

        public T GetByPk(int id)
        {
            return models.SingleOrDefault(s => s.Id == id);
        }
    }
}