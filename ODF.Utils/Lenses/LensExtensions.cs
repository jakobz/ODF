using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public static class LensExtensions
    {
        public static IPureLens<A, C> Sequence<A,B,C>(this IPureLens<A,B> first, IPureLens<B, C> second)
        {
            return Lens.Pure<A, C>(
                    a => second.Map(first.Map(a)),
                    c => first.UnMap(second.UnMap(c))
               );
        }
    }
}
