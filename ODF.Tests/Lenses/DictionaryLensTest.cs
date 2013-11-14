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
    public class DictionaryLensTest
    {
        class Model
        {
            public IDictionary<int, ModelItem> Items = new Dictionary<int, ModelItem>();
        }

        class ModelItem
        {
            public int Number { get; set; }
        }

        class View
        {
            public IDictionary<int, ChildItem> Items { get; set; }
        }

        class ChildItem
        {
            public string Number { get; set; }
        }

        static IMutateLens<ModelItem, ChildItem> childLens = Lens.Build<ModelItem, ChildItem>()
            .Scalar(m => m.Number, p => p.Number, Lens.IntToString)
            .Build();

        static IMutateLens<Model, View> lens = 
            Lens.Build<Model, View>()
            .Reference(m => m.Items, v => v.Items, Lens.Dictionary<int, ModelItem, ChildItem>(childLens, () => new ModelItem()))
            .Build();

        Model GetTestModel()
        {
            var model = new Model();
            model.Items[0] = new ModelItem() { Number = 0 };
            model.Items[1] = new ModelItem() { Number = 1 };
            return model;
        }

        [Test]
        public void BasicTest()
        {
            var model = GetTestModel();
            var view = lens.Map(model);

            Assert.AreEqual("0", view.Items[0].Number);
            Assert.AreEqual("1", view.Items[1].Number);
            Assert.AreEqual(2, view.Items.Count);

            view.Items[0].Number = "100";
            view.Items.Remove(1);
            view.Items[2] = new ChildItem() { Number = "200" };
            lens.Apply(model, view);

            Assert.AreEqual(100, model.Items[0].Number);
            Assert.IsFalse(model.Items.ContainsKey(1));
            Assert.AreEqual(200, model.Items[2].Number);
            Assert.AreEqual(2, model.Items.Count);
        }
    }
}
