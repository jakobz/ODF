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
    public class ComplexMappingTest 
    {
        interface IHasNumber
        {
            int Number { get; }
        }

        class Model : IHasNumber
        {
            public int Number { get; set; }
        }

        class View
        {
            public int Number { get; set; }
            public int Square { get; set; }
            public int InterfaceCube { get; set; }
        }
        
        static int CubeOverInterface(IHasNumber numberHolder)
        {
            return (int)Math.Pow(numberHolder.Number, 3);
        }

        [Test]
        public void InterfaceMap()
        {
            var lens = Lens.Build<Model, View>()
                .Scalar(m => m.Number, v => v.Number)
                .Project(m => m.Number * m.Number, v => v.Square)
                .Project(i => CubeOverInterface(i), v => v.InterfaceCube)
                .Build();

            var model = new Model()
            {
                Number = 10
            };

            var view = lens.Map(model);

            Assert.That(view.Number, Is.EqualTo(10));
            Assert.That(view.Square, Is.EqualTo(100));
            Assert.That(view.InterfaceCube, Is.EqualTo(1000));
        }
    }
}
