using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innovt.Core.Utilities;
using NUnit.Framework;
namespace Innovt.Core.Test
{
    public class ExtensionsTests
    {

        [Test]
        public void CheckRemoveAccents()
        {
            var guid = Guid.NewGuid();

            var strGuid = guid.ToString().Replace("-", "");


           Assert.IsNull(strGuid);
        }



    }
}
