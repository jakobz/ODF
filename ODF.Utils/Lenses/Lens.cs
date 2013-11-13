using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public static class Lens
    {
        public static LensBuilder<M, P> Build<M, P>()
        {
            return new LensBuilder<M, P>();
        }

        class IdentityLens<T> : ILens<T, T>
        {
            public T Get(T model)
            {
                return model;
            }

            public T Update(T model, T projection)
            {
                return projection;
            }
        }

        public static ILens<T, T> Identity<T>()
        {
            return new IdentityLens<T>();
        }
    }
}
