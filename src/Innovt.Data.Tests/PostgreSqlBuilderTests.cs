using Innovt.Core.Cqrs.Queries;
using Innovt.Data.Model;
using Innovt.Data.QueryBuilders;
using Innovt.Data.QueryBuilders.Builders;
using NUnit.Framework;

namespace Innovt.Data.Tests
{
    public class PostgreSqlBuilderTests
    {

        [Test]
        [TestCase(false,"SELECT id,name FROM [User]")]
        [TestCase(true,@"SELECT ""Id"",""Name"" FROM [User]")]
        public void CheckCamelCaseSyntax(bool respectColumnSyntax, string expected)
        {
            IQueryBuilder qBuilder = new PostgreSqlQueryBuilder() {RespectColumnSyntax = respectColumnSyntax};

            var actualSql = qBuilder.From("User").Select("Id", "Name").Sql();

            Assert.AreEqual(expected,actualSql);
        }

        [Test]
        [TestCase(true,"SELECT id,name FROM [User] WITH(NOLOCK)")]
        [TestCase(false,"SELECT id,name FROM [User]")]
        public void CheckNoLock(bool useNoLock, string expected)
        {
            IQueryBuilder qBuilder = new PostgreSqlQueryBuilder() {UseNoLock = useNoLock, RespectColumnSyntax = false};

            var actualSql = qBuilder.From("User",useNoLock:useNoLock).Select("id", "name").Sql();

            Assert.AreEqual(expected,actualSql);
        }


        [Test]
        public void ShouldReturnPagination_When_FilterIsProvided()
        {
            IQueryBuilder qBuilder = new PostgreSqlQueryBuilder() {RespectColumnSyntax = false};

            var expected = "SELECT id,name FROM [User] WHERE Id=1 ORDER BY name ASC OFFSET (0) LIMIT 10";

            var pagination = new PagedFilterBase() {Page = 1, PageSize = 10};

            var actualSql = qBuilder.Select("Id,Name").From("User").Where("Id=1").OrderBy(new OrderBy(true, "Name")).Paginate(pagination).Sql();

            Assert.AreEqual(expected,actualSql);
        }
    }
}