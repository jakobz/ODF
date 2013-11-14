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

        public static IMutateLens<IDictionary<Key, ModelValue>, IDictionary<Key, ProjectionValue>>
            Dictionary<Key, ModelValue, ProjectionValue>(IMutateLens<ModelValue, ProjectionValue> valueLens, Func<ModelValue> create)
        {
            return new DictionaryLens<Key, ModelValue, ProjectionValue>(valueLens, create);
        }

        public static readonly IPureLens<int, string> IntToString = Lens.Pure<int, string>(n => n.ToString(), b => int.Parse(b));
    }
}
