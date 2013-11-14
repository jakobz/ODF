using NUnit.Framework;
using ODF.Utils.Lenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Tests.Lenses
{
    [TestFixture]
    public class PropertyLensTests
    {
        class TestClass
        {
            public int IntProp { get; set; }
            public string StringProp { get; set; }
            public Child Child { get; set; }
        }

        class Child
        {
            public string ChildStringProp { get; set; }
        }

        [Test]
        public void StringProperty()
        {
            var obj = new TestClass()
            {
                StringProp = "initial"
            };

            var lens = new PropertyLens<TestClass, string>(m => m.StringProp);

            Assert.AreEqual("initial", obj.StringProp);
            Assert.AreEqual("initial", lens.Map(obj));
            lens.Update(obj, "new");
            Assert.AreEqual("new", obj.StringProp);
            Assert.AreEqual("new", lens.Map(obj));
        }

        [Test]
        public void IntProperty()
        {
            var obj = new TestClass()
            {
                IntProp = 3
            };

            var lens = new PropertyLens<TestClass, int>(m => m.IntProp);

            Assert.AreEqual(3, obj.IntProp);
            Assert.AreEqual(3, lens.Map(obj));
            lens.Update(obj, 42);
            Assert.AreEqual(42, obj.IntProp);
            Assert.AreEqual(42, lens.Map(obj));
        }

        [Test]
        public void ChildProperty()
        {
            var obj = new TestClass()
            {
                Child = new Child()
                {
                    ChildStringProp = "initial"
                }
            };

            var lens = new PropertyLens<TestClass, string>(m => m.Child.ChildStringProp);

            Assert.AreEqual(obj.Child.ChildStringProp, lens.Map(obj));
            lens.Update(obj, "new");
            Assert.AreEqual("new", obj.Child.ChildStringProp);
        }

    }
}
