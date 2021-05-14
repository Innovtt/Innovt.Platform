// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-12
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Collections;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Core.Specification;
using Innovt.Domain.Security;

namespace Innovt.Authorization.Platform.Infrastructure
{
    public class SecurityRepository: ISecurityRepository
    {
        public void Add(SecurityGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(SecurityGroup entity)
        {
            throw new NotImplementedException();
        }

        public void Modify(SecurityGroup entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(IEnumerable<SecurityGroup> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(SecurityGroup entity)
        {
            throw new NotImplementedException();
        }

        public SecurityGroup GetSingleOrDefault(ISpecification<SecurityGroup> specification, Include includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<SecurityGroup> GetSingleOrDefaultAsync(ISpecification<SecurityGroup> specification, Include includes = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public SecurityGroup GetFirstOrDefault(ISpecification<SecurityGroup> specification, Include includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<SecurityGroup> GetFirstOrDefaultAsync(ISpecification<SecurityGroup> specification, Include includes = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SecurityGroup> FindBy(ISpecification<SecurityGroup> specification, Include includes = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SecurityGroup> FindBy<TKey>(ISpecification<SecurityGroup> specification, Expression<Func<SecurityGroup, TKey>> orderBy = null, bool isOrderByDescending = false,
            Include includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SecurityGroup>> FindByAsync(ISpecification<SecurityGroup> specification, Include includes = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SecurityGroup>> FindByAsync<TKey>(ISpecification<SecurityGroup> specification, Expression<Func<SecurityGroup, TKey>> orderBy = null, bool isOrderByDescending = false,
            Include includes = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public PagedCollection<SecurityGroup> FindPaginatedBy(ISpecification<SecurityGroup> specification, Include includes = null)
        {
            throw new NotImplementedException();
        }

        public PagedCollection<SecurityGroup> FindPaginatedBy<TKey>(ISpecification<SecurityGroup> specification, Expression<Func<SecurityGroup, TKey>> orderBy = null,
            bool isOrderByDescending = false, Include includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<PagedCollection<SecurityGroup>> FindPaginatedByAsync(ISpecification<SecurityGroup> specification, Include includes = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedCollection<SecurityGroup>> FindPaginatedByAsync<TKey>(ISpecification<SecurityGroup> specification, Expression<Func<SecurityGroup, TKey>> orderBy = null,
            bool isOrderByDescending = false, Include includes = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public int CountBy<TKEntity>(ISpecification<TKEntity> specification) where TKEntity : class
        {
            throw new NotImplementedException();
        }

        public int CountBy(ISpecification<SecurityGroup> specification)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountByAsync<TKEntity>(ISpecification<TKEntity> specification, CancellationToken cancellationToken = default) where TKEntity : class
        {
            throw new NotImplementedException();
        }

        public Task AddPolicy(Policy policy)
        {
            throw new NotImplementedException();
        }

        public Task RemovePolicy(Policy policy)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Policy>> GetPolicies(string name = null, string description = null)
        {
            throw new NotImplementedException();
        }

        public Task AddPermission(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task RemovePermission(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null, string name = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null)
        {
            throw new NotImplementedException();
        }
    }
}