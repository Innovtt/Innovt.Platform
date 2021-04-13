// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado.Tests
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Innovt.Data.Ado.Tests
{
    public class RepositoryBaseTests
    {
        private readonly UserRepository repository = null;

        [SetUp]
        public void Setup()
        {
            //var connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=AdoTestDB;Integrated Security=SSPI;AttachDBFilename=X:\Projects\Innovt.Platform\src\Innovt.Data.Ado.Tests\AdoTestDB.mdf";

            //repository = new UserRepository(new DefaultDataSource("TestDB",connectionString, Provider.MsSql));
        }


        [TearDown]
        public void TearDown()
        {
            repository.DeleteAllUsers();
        }


        [Test]
        public async Task Test1()
        {
            try
            {
                var users = await repository.GetAll();

                Assert.IsNotNull(users);

                Assert.Pass();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}