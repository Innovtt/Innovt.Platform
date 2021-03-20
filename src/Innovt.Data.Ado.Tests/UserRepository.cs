using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using Innovt.Data.Ado.Tests.Model;
using Innovt.Data.DataSources;

namespace Innovt.Data.Ado.Tests
{
    public class UserRepository : RepositoryBase
    {
        public UserRepository(IDataSource datasource) : base(datasource)
        {
        }

        public UserRepository(IDataSource datasource, IConnectionFactory connectionFactory) : base(datasource,
            connectionFactory)
        {
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var query = "SELECT * FROM USER ORDER BY NAME";

            var filter = new PagedFilterBase() {Page = 1, PageSize = 10};

            var res = await QueryPagedAsync<User>(query, filter);


            return await QueryAsync<User>(query);
        }

        public void DeleteAllUsers()
        {
            //base.CreateQuery("User").in
        }
    }
}