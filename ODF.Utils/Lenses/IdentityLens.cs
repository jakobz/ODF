using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    class IdentityLens<T> : IPureLens<T, T>
    {
        public T Map(T from)
        {
            return from;
        }

        public T UnMap(T to)
        {
            return to;
        }
    }
}
