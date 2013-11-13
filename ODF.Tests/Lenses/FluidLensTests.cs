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
    public class FluidLensTests
    {
        class Model
        {
            public int ID { get; set; }
            public string Name { get; set;}
            public string Description { get; set; }
            public ModelChild Child { get; set; }
        }

        class ModelChild
        {
            public string ChildName { get; set; }
        }

        class Projection
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public ProjectionChild ProjChild { get; set; }
        }

        class ProjectionChild
        {
            public string Name { get; set; }
        }

        static ILens<ModelChild, ProjectionChild> childLens = Lens.Build<ModelChild, ProjectionChild>()
            .Map(m => m.ChildName, v => v.Name, Lens.Identity<string>())
            .Build();

        static ILens<Model, Projection> staticLens = Lens.Build<Model, Projection>()
                .Map(m => m.ID, v => v.ID, Lens.Identity<int>(), IsReadonly: true)
                .Map(m => m.Name, v => v.Name, Lens.Identity<string>())
                .Map(m => m.Child, v => v.ProjChild, childLens)
                .Build();

        [Test]
        public void BasicMapping()
        {
            var model = new Model();
            var view = staticLens.Get(model);
            staticLens.Update(model, view);
        }
    }
}
