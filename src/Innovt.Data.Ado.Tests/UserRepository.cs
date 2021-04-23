using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using Innovt.Data.Ado.Tests.Model;
using Innovt.Data.DataSources;

namespace Innovt.Data.Ado.Tests
{
    public class UserRepository:RepositoryBase
    {
        public UserRepository(IDataSource datasource) : base(datasource)
        {
        }

        public UserRepository(IDataSource datasource, IConnectionFactory connectionFactory) : base(datasource, connectionFactory)
        {
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var query = "SELECT s.ID_BRANCH AS Id, s.ID_VENDOR AS IdVendor,  s.NAME AS Name, s.CORPORATE_NAME AS CorporateName, s.NUM_DOCUMENT AS Document, s.LAST_UPDATE AS LastUpdate FROM APPS.XXATCP_AP_SUPPLIERS_V s WHERE 1=1 ORDER BY s.ID_BRANCH";

            var filter = new PagedFilterBase() { Page = 1, PageSize = 100};
            
            var res = await base.QueryPagedAsync<User>(query,filter);
            
            return await base.QueryAsync<User>(query);
        }

        public void DeleteAllUsers()
        {

            //base.CreateQuery("User").in
        }
    }
}