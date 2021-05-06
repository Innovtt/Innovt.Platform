// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado.Tests
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading.Tasks;
using Innovt.Data.DataSources;
using Innovt.Data.Model;
using NUnit.Framework;

namespace Innovt.Data.Ado.Tests
{
    public class RepositoryBaseTests
    {
        private UserRepository repository;

        [SetUp]
        public void Setup()
        {
            var connectionString =
                @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=AdoTestDB;Integrated Security=SSPI;AttachDBFilename=X:\Projects\Innovt.Platform\src\Innovt.Data.Ado.Tests\AdoTestDB.mdf";

            repository = new UserRepository(new DefaultDataSource("TestDB", connectionString, Provider.Oracle));
        }


        [TearDown]
        public void TearDown()
        {
            
        }


        [Test]
        [Ignore("Internal tests")]
        public async Task Test1()
        {
            try
            {
                var value = DateTime.Now;



                var weekDay = value.ToString("dddd").Split('-');

                var dia = weekDay[0][0..1].ToUpper() + weekDay[0][1..];
                
                var dia2 = weekDay[0][0..1].ToUpper() + weekDay[0][1..];


                var users = await repository.GetAll();

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