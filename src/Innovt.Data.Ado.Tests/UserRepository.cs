// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado.Tests
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Data.DataSources;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<dynamic>> CountBy()
        {
            var query = new StringBuilder(@"SELECT 
                            B.""Id"",
                            BG.""Name"" AS BuyerGroup,
                            BG.""Id"" AS BuyerGroupId,
                            B.""Document"",
                            B.""Name"",
                            B.""CorporateName"",
                            B.""ErpId"",
                            B.""Document"",
                            C.""Enabled"",
                            C.""Id"" AS ConfigurationId,
                            C.""AccessKey"", C.""Enabled"", C.""IntegrationInterval"", C.""PingInterval"", TO_CHAR(C.""StartIntegration"",'HH24:MI:SS') AS StartIntegration, TO_CHAR(C.""EndIntegration"" ,'HH24:MI:SS') AS EndIntegration, CASE WHEN  C.""StartMaintenance"" IS NULL THEN NULL ELSE  TO_CHAR(C.""StartMaintenance"" ,'HH24:MI:SS') END AS StartMaintenance ,  CASE WHEN  C.""EndMaintenance"" IS NULL THEN NULL ELSE  TO_CHAR(C.""EndMaintenance"" ,'HH24:MI:SS') END AS EndMaintenance , C.""Page"" , C.""PageSize"" , C.""DaysIteratorInterval"" , C.""IsAntecipaClient""  , C.""Parameters"" , C.""ErpId"", C.""VersionId"" , C.""ConnectorId""  , C.""ConnectionString""   , C.""Parameters"" 
                        FROM public.""Buyer"" AS B
                        INNER JOIN public.""BuyerGroup"" AS BG ON BG.""Id"" = B.""BuyerGroupId""
                        INNER JOIN public.""Configuration"" AS C ON B.""ConfigurationId"" = C.""Id""
                        WHERE 1=1  AND B.""Id""='ffc04a95-15d6-4a32-9dcc-381a996e37c5'");

            return await QueryAsync<dynamic>(query.ToString()).ConfigureAwait(true);
        }

        public async Task<IEnumerable<dynamic>> GetAll()
        {
            var query = new StringBuilder(@"SELECT 
                            B.""Id"",
                            BG.""Name"" AS BuyerGroup,
                            BG.""Id"" AS BuyerGroupId,
                            B.""Document"",
                            B.""Name"",
                            B.""CorporateName"",
                            B.""ErpId"",
                            B.""Document"",
                            C.""Enabled"",
                            C.""Id"" AS ConfigurationId,
                            C.""AccessKey"", C.""Enabled"", C.""IntegrationInterval"", C.""PingInterval"", TO_CHAR(C.""StartIntegration"",'HH24:MI:SS') AS StartIntegration, TO_CHAR(C.""EndIntegration"" ,'HH24:MI:SS') AS EndIntegration, CASE WHEN  C.""StartMaintenance"" IS NULL THEN NULL ELSE  TO_CHAR(C.""StartMaintenance"" ,'HH24:MI:SS') END AS StartMaintenance ,  CASE WHEN  C.""EndMaintenance"" IS NULL THEN NULL ELSE  TO_CHAR(C.""EndMaintenance"" ,'HH24:MI:SS') END AS EndMaintenance , C.""Page"" , C.""PageSize"" , C.""DaysIteratorInterval"" , C.""IsAntecipaClient""  , C.""Parameters"" , C.""ErpId"", C.""VersionId"" , C.""ConnectorId""  , C.""ConnectionString""   , C.""Parameters"" 
                        FROM public.""Buyer"" AS B
                        INNER JOIN public.""BuyerGroup"" AS BG ON BG.""Id"" = B.""BuyerGroupId""
                        INNER JOIN public.""Configuration"" AS C ON B.""ConfigurationId"" = C.""Id""
                        WHERE 1=1  AND B.""Id""='ffc04a95-15d6-4a32-9dcc-381a996e37c5'");

            return await QueryAsync<dynamic>(query.ToString()).ConfigureAwait(true);
        }
    }
}