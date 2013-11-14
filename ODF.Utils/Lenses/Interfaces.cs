using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public interface IMap<From, To>
    {
        To Map(From from);
    }

    public interface IPureLens<From, To> : IMap<From, To>
    {
        From UnMap(To to);
    }

    public interface IMutateLens<From, To> : IMap<From, To>
    {
        void Apply(From model, To projection);
    }
}
