// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Ado.Tests

using System.Threading.Tasks;
using Innovt.Data.Ado.Tests.Model;
using Innovt.Data.DataSources;
using NUnit.Framework;

namespace Innovt.Data.Ado.Tests;

[TestFixture]
public class RepositoryBaseTests
{
    [Test]
    [Ignore("Only for Local Tests")]
    public async Task Repository()
    {
        var query = @"SELECT a.*
                                FROM
                                  (SELECT DISTINCT ar.CorrelationId AS Id,
                                                   ar.Id AS Number,
                                                   CAST(Ar.MaxDiscountRate AS DECIMAL(18, 4)) AS MaximumDiscountRate,
                                                   CAST(Ar.EffectiveDiscountRate AS DECIMAL(18, 4)) AS EffectiveDiscountRate,
                                                   u.FirstName + ' '+ u.LastName AS RequestedBy,
                                                   ar.CreatedOn AS RequestedOn,
                                                   ar.Type,
                                                   CASE ar.Type
                                                       WHEN 1 THEN 'Data Limite'
                                                       WHEN 2 THEN 'Automática'
                                                       ELSE 'Manual'
                                                   END TypeDescription,
                                                   CAST(x.AnticipatedAmount AS DECIMAL(18, 2)) AS AnticipatedAmount,
                                                   CASE ar.Type
                                                       WHEN 3 THEN CAST(z.RequestedAmount AS DECIMAL(18, 2))
                                                       ELSE CAST(x.GrossAmount AS DECIMAL(18, 2))
                                                   END AS GrossAmount,
                                                   CAST(x.Discount AS DECIMAL(18, 2)) AS Discount,
                                                   dbo.StatusAntecipationRequest(ar.Id) AS Status,
                                                   ar.CreatedOn
                                   FROM dbo.AntecipationRequest ar WITH (NOLOCK)
                                   INNER JOIN dbo.Company c WITH (NOLOCK) ON ar.CompanyId = c.Id
                                   INNER JOIN dbo.[User] U WITH (NOLOCK) ON U.Id = ar.RequestedById
                                   LEFT JOIN
                                     (SELECT vrar.AntecipationRequestId,
                                             SUM(vrpo.NetValue+vrpo.Discount) AS GrossAmount,
                                             SUM(Vrpo.Discount) AS Discount,
                                             SUM(vrpo.NetValue) AS AnticipatedAmount
                                      FROM dbo.VickreyResultAntecipationRequest vrar WITH (NOLOCK)
                                      INNER JOIN dbo.VickreyResultPaymentOrder vrpo WITH (NOLOCK) ON vrar.VickreyResultId = vrpo.VickreyResultId
                                      AND vrar.AntecipationRequestId = vrpo.AntecipationRequestId
                                      INNER JOIN dbo.AntecipationPaymentOrder apo WITH (NOLOCK) ON vrpo.PaymentOrderId = apo.PaymentOrderId
                                      AND vrpo.VickreyResultId = apo.VickreyResultId
                                      AND vrpo.AntecipationRequestId = apo.AntecipationRequestId
                                      WHERE apo.AntecipationPaymentOrderStatusId=2
                                      GROUP BY vrar.AntecipationRequestId) x ON x.AntecipationRequestId=ar.Id  
                                    LEFT JOIN
                                      (SELECT arpo.AntecipationRequestId,
                                              COUNT(arpo.PaymentOrderId) AS CountRequested,
                                              SUM(arpo.NetValue) AS RequestedAmount
                                       FROM dbo.PaymentOrder po WITH (NOLOCK)
                                       INNER JOIN dbo.AntecipationRequestPaymentOrder arpo WITH (NOLOCK) ON arpo.PaymentOrderId=po.Id
                                       GROUP BY arpo.AntecipationRequestId) z ON z.AntecipationRequestId=ar.Id
                                   WHERE c.NewId=@CompanyId ) a
                                WHERE 1=1  ORDER BY a.CreatedOn DESC ";

        var dataSource = new DataSourceReader("FakeDb", "Fake");

        var repo = new UserRepository(dataSource);


        var filter = new UserFilter();

        var res = await repo.QueryPagedAsync<User>(query, filter).ConfigureAwait(false);
    }
}