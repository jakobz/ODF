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
            public int ProjectionID { get; set; }
            public string ProjectionName { get; set; }
            public ProjectionChild ProjectionChild { get; set; }
        }

        class FlatProjection
        {
            public string ChildName { get; set; }
        }

        class ProjectionChild
        {
            public string Name { get; set; }
        }

        static IMutateLens<ModelChild, ProjectionChild> childLens = Lens.Build<ModelChild, ProjectionChild>()
            .Scalar(m => m.ChildName, v => v.Name)
            .Build();

        static IMutateLens<Model, Projection> staticLens = Lens.Build<Model, Projection>()
                .Scalar(m => m.ID, v => v.ProjectionID)
                .Scalar(m => m.Name, v => v.ProjectionName)
                .Reference(m => m.Child, v => v.ProjectionChild, childLens)
                .Build();

        Model GetTestModel()
        {
            return new Model()
            {
                ID = 10,
                Name = "name",
                Description = "description",
                Child = new ModelChild()
                {
                    ChildName = "child name"
                }
            };
        }

        [Test]
        public void BasicMapping()
        {
            var model = GetTestModel();
            var view = staticLens.Map(model);
            Assert.AreEqual(10, view.ProjectionID);
            Assert.AreEqual("name", view.ProjectionName);
            Assert.AreEqual("child name", view.ProjectionChild.Name);

            view.ProjectionID = 11;
            view.ProjectionName = "new name";
            view.ProjectionChild.Name = "new child name";

            staticLens.Apply(model, view);

            Assert.AreEqual(11, model.ID);
            Assert.AreEqual("new name", model.Name);
            Assert.AreEqual("new child name", model.Child.ChildName);
            Assert.AreEqual("description", model.Description);

            var anotherModel = GetTestModel();

            staticLens.Apply(anotherModel, view);

            Assert.AreEqual(11, anotherModel.ID);
            Assert.AreEqual("new name", anotherModel.Name);
            Assert.AreEqual("new child name", anotherModel.Child.ChildName);
            Assert.AreEqual("description", anotherModel.Description);
        }

        [Test]
        public void FlattenTest()
        {
            var model = GetTestModel();
            var lens = Lens.Build<Model, FlatProjection>().Scalar(m => m.Child.ChildName, p => p.ChildName, Lens.Identity<string>()).Build();
            var view = lens.Map(model);
            Assert.AreEqual("child name", view.ChildName);
            view.ChildName = "new child name";
            lens.Apply(model, view);
            Assert.AreEqual("new child name", model.Child.ChildName);
        }
    }
}
