using Innovt.Core.Cqrs.Queries;
using Innovt.Data.Model;
using Innovt.Data.QueryBuilders;
using Innovt.Data.QueryBuilders.Builders;
using NUnit.Framework;

namespace Innovt.Data.Tests
{
    public class SqlBuilderTests
    {

        [Test]
        [TestCase(false,"SELECT ID,NAME FROM [User] WITH(NOLOCK)")]
        [TestCase(true,"SELECT id,Name FROM [User] WITH(NOLOCK)")]
        public void MssqlSelect_CheckColumnSyntax(bool respectColumnSyntax, string expected)
        {
            IQueryBuilder qBuilder = new MsSqlQueryBuilder() {RespectColumnSyntax = respectColumnSyntax};

            var actualSql = qBuilder.From("User").Select("id", "Name").Sql();

            Assert.AreEqual(expected,actualSql);
        }

        [Test]
        [TestCase(true,"SELECT ID,NAME FROM [User] WITH(NOLOCK)")]
        [TestCase(false,"SELECT ID,NAME FROM [User]")]
        public void MssqlSelect_CheckNoLock(bool useNoLock, string expected)
        {
            IQueryBuilder qBuilder = new MsSqlQueryBuilder() {UseNoLock = useNoLock, RespectColumnSyntax = false};

            var actualSql = qBuilder.From("User",useNoLock:useNoLock).Select("id", "name").Sql();

            Assert.AreEqual(expected,actualSql);
        }


        [Test]
        public void MssqlSelect_ShouldReturn_Without_Where()
        {
            IQueryBuilder qBuilder =new MsSqlQueryBuilder();

            var expected = "SELECT id,name FROM [User] WITH(NOLOCK)";

            var actualSql = qBuilder.From("User").Select("id", "name").Sql();

            Assert.AreEqual(expected,actualSql);
        }


        [Test]
        public void MssqlSelect_ShouldReturn_With_Where()
        {
            IQueryBuilder qBuilder =new MsSqlQueryBuilder();
            var expected = "SELECT id,name FROM [User] WITH(NOLOCK) WHERE Id=1";
            
            var actualSql = qBuilder.Select("id", "name").From("User").Where("Id=1").Sql();

            Assert.AreEqual(expected,actualSql);
        }


        [Test]
        public void MssqlSelect_Count_Without_Where()
        {
            IQueryBuilder qBuilder = new MsSqlQueryBuilder();

            var expected = "SELECT COUNT(1) FROM [User] WITH(NOLOCK)";

            var actualSql = qBuilder.Count().From("User").Sql();

            Assert.AreEqual(expected,actualSql);
        }

        
        [Test]
        public void MssqlSelect_Count_With_Where()
        {
            IQueryBuilder qBuilder =new MsSqlQueryBuilder();

            var expected = "SELECT COUNT(1) FROM [Sale] WITH(NOLOCK) WHERE Id=1";

            var actualSql = qBuilder.Count().From("Sale").Where("Id=1").Sql();

            Assert.AreEqual(expected,actualSql);
        }

          
        [Test]
        public void MssqlSelect_Top_With_Where()
        {
            IQueryBuilder qBuilder = new MsSqlQueryBuilder();

            var expected = "SELECT TOP 1 Id,Name FROM [User] WITH(NOLOCK) WHERE Id=1";

            var actualSql = qBuilder.Select("Id,Name").Top(1).From("User").Where("Id=1").Sql();

            Assert.AreEqual(expected,actualSql);
        }

        [Test]
        public void Mssql_Pagination()
        {
            IQueryBuilder qBuilder = new MsSqlQueryBuilder();

            var expected = "SELECT Id,Name FROM [User] WITH(NOLOCK) WHERE Id=1 ORDER BY Name ASC OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY";

            var pagination = new PagedFilterBase() {Page = 1, PageSize = 10};

            var actualSql = qBuilder.Select("Id,Name").From("User").Where("Id=1").OrderBy(new OrderBy(true,"Name")).Paginate(pagination).Sql();

            Assert.AreEqual(expected,actualSql);
        }
    }
}