using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class LambdaMap<From, To> : IMap<From, To>
    {
        Func<From, To> map;

        public LambdaMap(Func<From, To> map)
        {
            this.map = map;
        }

        public To Map(From from)
        {
            return map(from);
        }
    }
}
