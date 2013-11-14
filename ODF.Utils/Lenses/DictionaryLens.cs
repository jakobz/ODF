using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class DictionaryLens<K, FromV, ToV> : IMutateLens<IDictionary<K, FromV>, IDictionary<K, ToV>>
    {
        IMutateLens<FromV, ToV> valueLens;
        Func<FromV> create;

        public DictionaryLens(IMutateLens<FromV, ToV> valueLens, Func<FromV> create)
        {
            this.valueLens = valueLens;
            this.create = create;
        }

        public IDictionary<K, ToV> Map(IDictionary<K, FromV> from)
        {
            return from.ToDictionary(i => i.Key, i => valueLens.Map(i.Value));
        }

        public void Apply(IDictionary<K, FromV> model, IDictionary<K, ToV> projection)
        {
            var keys = model.Keys.Concat(projection.Keys).Distinct().ToList();

            foreach (var key in keys)
            {
                var inModel = model.ContainsKey(key);
                var inProjection = projection.ContainsKey(key);

                if (inModel)
                {
                    if (inProjection)
                    {
                        valueLens.Apply(model[key], projection[key]);
                    }
                    else
                    {
                        model.Remove(key);
                    }
                }
                else 
                {
                    var newItem = create();
                    valueLens.Apply(newItem, projection[key]);
                    model[key] = newItem;
                }
            }
        }
    }
}
