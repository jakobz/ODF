using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class BasicLens<From, To> : IPureLens<From, To>
    {
        Func<From, To> towards;
        Func<To, From> backwards;

        public BasicLens(Func<From, To> towards, Func<To, From> backwards)
        {
            this.towards = towards;
            this.backwards = backwards;
        }

        public To Map(From from)
        {
            return towards(from);
        }

        public From UnMap(To to)
        {
            return backwards(to);
        }
    }
}
