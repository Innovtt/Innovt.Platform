using Innovt.Data.DataSources;
using NUnit.Framework;

namespace Innovt.Data.Ado.Tests
{
    public class RepositoryBaseTests
    {
        private RepositoryBase repository;

        [SetUp]
        public void Setup()
        {
            repository = new RepositoryBase(new DefaultDataSource("memory"));
        }

        [Test]
        public void Test1()
        {
            //repository.QuerySingleOrDefaultAsync<dynamic>("",)
=
            Assert.Pass();
        }
    }
}