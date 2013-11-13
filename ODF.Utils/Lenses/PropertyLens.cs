using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils.Lenses
{
    public class PropertyLens<M,P> : ILens<M, P>
    {
        Func<M, P> getter;
        Action<M, P> setter;

        public PropertyLens(Expression<Func<M, P>> expression)
        {
            getter = expression.Compile();

            var member = (MemberExpression)expression.Body;
            var param = Expression.Parameter(typeof(P), "value");
            setter = Expression.Lambda<Action<M, P>>(Expression.Assign(member, param), expression.Parameters[0], param).Compile();
        }

        public P Get(M model)
        {
            return getter(model);
        }

        public M Update(M model, P projection)
        {
            setter(model, projection);
            return model;
        }
    }
}
