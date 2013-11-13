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

        public static ILens<T, T> Identity<T>()
        {
            return new IdentityLens<T>();
        }
    }
}
