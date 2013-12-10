using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Tests.Lenses
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void DynamicToJson()
        {
            dynamic obj = new
            {
                ID = 10,
                List = new object[] { 1, 2, new { Prop = 20 } }
            };

            var json = JsonConvert.SerializeObject(obj);

            Console.Write(json);
        }
    }
}
