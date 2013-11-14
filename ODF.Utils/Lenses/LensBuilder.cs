using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class LensBuilder<M, P> where P: new()
    {
        class PropertyMapping
        {
            public Action<M, P> CopyTowards;
            public Action<M, P> CopyBackwards;
        }

        class ObjectLens : IMutateLens<M, P>
        {
            public List<PropertyMapping> mappings = new List<PropertyMapping>();

            public P Map(M model)
            {
                P result = new P();

                foreach (var mapping in mappings)
                {
                    mapping.CopyTowards(model, result);
                }

                return result;
            }

            public void Update(M model, P projection)
            {
                foreach (var mapping in mappings)
                {
                    mapping.CopyBackwards(model, projection);
                }
            }
        }

        ObjectLens lens = new ObjectLens();
 
        public LensBuilder<M, P> Scalar<MProp, PProp>(
            Expression<Func<M, MProp>> modelProperty,
            Expression<Func<P, PProp>> viewProperty,
            IPureLens<MProp, PProp> transformLens
            )
        {
            var modelLens = new PropertyLens<M, MProp>(modelProperty);
            var viewLens = new PropertyLens<P, PProp>(viewProperty);
            lens.mappings.Add(new PropertyMapping()
            {
                CopyTowards = (from, to) =>
                {
                    var val = modelLens.Map(from);
                    var converted = transformLens.Map(val);
                    viewLens.Update(to, converted);
                },
                CopyBackwards = (from, to) =>
                {
                    var val = viewLens.Map(to);
                    var converted = transformLens.UnMap(val);
                    modelLens.Update(from, converted);
                }
            });
            return this;
        }

        public LensBuilder<M, P> Reference<MProp, PProp>(
            Expression<Func<M, MProp>> modelProperty,
            Expression<Func<P, PProp>> viewProperty,
            IMutateLens<MProp, PProp> transformLens
            )
        {
            var modelLens = new PropertyLens<M, MProp>(modelProperty);
            var viewLens = new PropertyLens<P, PProp>(viewProperty);
            lens.mappings.Add(new PropertyMapping()
            {
                CopyTowards = (from, to) =>
                {
                    var val = modelLens.Map(from);
                    var converted = transformLens.Map(val);
                    viewLens.Update(to, converted);
                },
                CopyBackwards = (from, to) =>
                {
                    var toObj = viewLens.Map(to);
                    var fromObj = modelLens.Map(from);
                    transformLens.Update(fromObj, toObj);
                }
            });
            return this;
        }

        public LensBuilder<M, P> Identity<T>(
            Expression<Func<M, T>> modelProperty,
            Expression<Func<P, T>> viewProperty)
        {
            return Scalar<T, T>(modelProperty, viewProperty, Lens.Identity<T>());
        }

        public IMutateLens<M, P> Build()
        {
            return lens;
        }
    }
}
