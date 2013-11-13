using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class LensBuilder<M, P>
    {
        public LensBuilder<M, P> Map<MProp, PProp>(
            Expression<Func<M, MProp>> modelProperty,
            Expression<Func<P, PProp>> viewProperty,
            ILens<MProp, PProp> propertyLens,
            bool IsReadonly = false)
        {
            return this;
        }

        public LensBuilder<M, P> Identity<T>(
            Expression<Func<M, T>> modelProperty,
            Expression<Func<P, T>> viewProperty)
        {
            return Map<T, T>(modelProperty, viewProperty, Lens.Identity<T>());
        }

        public ILens<M, P> Build()
        {
            return null;
        }
    }
}
