using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public static class Lens
    {
        public static LensBuilder<From, To> Build<From, To>() where To : new()
        {
            return new LensBuilder<From, To>();
        }

        public static IPureLens<T, T> Identity<T>()
        {
            return new IdentityLens<T>();
        }

        public static IPureLens<From, To> Pure<From, To>(Func<From, To> towards, Func<To, From> backwards)
        {
            return new BasicLens<From, To>(towards, backwards);
        }
    }
}
