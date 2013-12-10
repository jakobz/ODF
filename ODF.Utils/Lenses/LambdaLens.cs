using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class LambdaLens<From, To> : LambdaMap<From,To>, IPureLens<From, To>
    {
        Func<To, From> backwards;

        public LambdaLens(Func<From, To> towards, Func<To, From> backwards) : base(towards)
        {
            this.backwards = backwards;
        }

        public From UnMap(To to)
        {
            return backwards(to);
        }
    }
}
