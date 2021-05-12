// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core.Test
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.Serialization;
using Innovt.Core.Test.Models;
using NUnit.Framework;

namespace Innovt.Core.Test
{
    public class DeserializerFactoryTests
    {
        [Test]
        public void Instance_ShouldNotReturnNUll()
        {
            var instance = DeserializerFactory.Instance;

            Assert.IsNotNull(instance);
        }

        [Test]
        public void Instance_ShouldBeTheSame_After_ManyCalls()
        {
            var instanceA = DeserializerFactory.Instance;

            var instanceB = DeserializerFactory.Instance;

            Assert.IsNotNull(instanceA);
            Assert.IsNotNull(instanceB);

            Assert.AreEqual(instanceA, instanceB);
        }
        
        [Test]
        public void Deserialize_ShouldReturn_Null_If_Content_IsNull_Or_Empty()
        {
            var result = DeserializerFactory.Instance.Deserialize("A", null);
            
            Assert.IsNull(result);

            result = DeserializerFactory.Instance.Deserialize("A", "");

            Assert.IsNull(result);
        }


        [Test]
        public void Deserialize_ShouldReturn_Null_When_Has_No_Mapping()
        {
            var result = DeserializerFactory.Instance.Deserialize("A","");
            
            Assert.IsNull(result);

            //new mapping
            DeserializerFactory.Instance.AddMapping<A>("A");

            var result2 = DeserializerFactory.Instance.Deserialize("B", "B");

            Assert.IsNull(result2);
        }


        [Test]
        public void Deserialize()
        {
            //first check without mapping
            DeserializerFactory.Instance.AddMapping<A>("A").AddMapping<B>();

            var a = new A() { Age = 10, LastName = "Borges", Name = "Michel"};
            var aJsonContent = System.Text.Json.JsonSerializer.Serialize(a);

            //With Key 
            var resultA = DeserializerFactory.Instance.Deserialize("A", aJsonContent);
            
            Assert.IsNotNull(resultA);
            Assert.IsNotNull(resultA as A);

            var b = new B() { Document  = "12312", Id = Guid.Empty, Role = "Admin"};
            var bJsonContent = System.Text.Json.JsonSerializer.Serialize(b);

            //With Key 
            var resultB = DeserializerFactory.Instance.Deserialize(typeof(B).FullName, bJsonContent);

            Assert.IsNotNull(resultB);
            Assert.IsNotNull(resultB as B);
        }



    }
}