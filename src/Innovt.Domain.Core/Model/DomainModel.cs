// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Model;

public class DomainModel<T> : ValueObject where T : ValueObject
{
    private readonly List<T> models = new();

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