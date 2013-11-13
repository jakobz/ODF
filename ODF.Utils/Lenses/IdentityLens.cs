using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
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
}
