// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.EFCore
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Repository;

namespace Innovt.Data.EFCore.Repositories
{
    public class GenericRepository<T> : RepositoryBase<T> where T : class
    {
        public GenericRepository(IExtendedUnitOfWork context) : base(context)
        {
        }
    }
}