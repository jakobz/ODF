using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class CompositeLens<M, P> : ILens<M, P>
    {
        public P Get(M model)
        {
            throw new NotImplementedException();
        }

        public M Update(M model, P projection)
        {
            throw new NotImplementedException();
        }
    }
}
