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
            var query = CreateQuery().FromRaw("Invoice as i WITH (nolock)")
                .Select("cbs.CompanyId AS BuyerId", "i.CompanyBranchBuyerId AS BranchBuyerId", "cbs.BranchSupplierId AS BranchSupplierId", "cb.CompanyId AS SupplierId",
                    "i.Id", "i.ExternalId", "i.Value", "i.Detail", "i.InvoiceNumber", "i.InvoiceLetter", "i.IssueDate", "i.OnCreated", "i.OnUpdated", "i.NetValue")
              
                .Join("CompanyBranchSupplier AS cbs", "cbs.Id", "i.CompanyBranchSupplierId")
                .Join("CompanyBranch AS cb", "cb.Id", "cbs.BranchSupplierId")
                .Where("cbs.CompanyId","=",1)
                .OrderBy("i.Id");


          // var query = base.CreateQuery("User").Select("Id").Where("Id","=",1).OrderBy("Name");
            var filter = new PagedFilterBase() {Page = 1, PageSize = 10};

            //var query = CreateQuery("Invoice as i")
            //            .Select("cbs.CompanyId AS BuyerId","i.CompanyBranchBuyerId AS BranchBuyerId", "cbs.BranchSupplierId AS BranchSupplierId", "cb.CompanyId AS SupplierId")
            //    .Join("CompanyBranchSupplier AS cbs", "cbs.Id", "i.CompanyBranchSupplierId")
            //    .Join("CompanyBranch AS cb", "cb.Id", "cbs.BranchSupplierId")
            //    .OrderBy("i.Id");

            var res = await base.QueryPagedAsync<User>(query,filter);


            return await base.QueryAsync<User>(query);
        }

        public void DeleteAllUsers()
        {

            //base.CreateQuery("User").in
        }
    }
}