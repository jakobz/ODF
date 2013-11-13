using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public interface ILens<Model, Projection> 
    {
        Projection Get(Model model);
        Model Update(Model model, Projection projection);
    }
}
