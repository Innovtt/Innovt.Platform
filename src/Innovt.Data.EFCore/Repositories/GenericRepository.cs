// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using Innovt.Domain.Core.Repository;

namespace Innovt.Data.EFCore.Repositories;
/// <summary>
/// Generic repository implementation for accessing and managing entities of type T.
/// Inherits from RepositoryBase for shared functionality and extends for specific entity type.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class GenericRepository<T> : RepositoryBase<T> where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The extended unit of work context.</param>
    public GenericRepository(IExtendedUnitOfWork context) : base(context)
    {
    }
}