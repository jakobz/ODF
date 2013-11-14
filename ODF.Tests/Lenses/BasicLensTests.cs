using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ODF.Utils.Lenses;

namespace ODF.Tests.Lenses
{
    [TestFixture]
    public class PureLensTests
    {
        [Test]
        public void ConvertToInt()
        {
            var lens = Lens.Pure<int, string>(n => n.ToString(), b => int.Parse(b));

            Assert.AreEqual("10", lens.Map(10));
            Assert.AreEqual(10, lens.UnMap("10"));
        }

        [Test]
        public void BasicCompose()
        {
            var intToString = Lens.Pure<int, string>(n => n.ToString(), b => int.Parse(b));
            var stringToInt = Lens.Pure<string, int>(s => int.Parse(s), b => b.ToString());
            var lens = intToString.Sequence(stringToInt);

            Assert.AreEqual(10, lens.Map(10));
            Assert.AreEqual(20, lens.UnMap(20));
        }
    }
}
