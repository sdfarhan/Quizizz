using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Quizizz.Services.Tools.Expressions
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        public Expression<Func<T, bool>> GetExpression<T>(string queryType, string queryValue, string roleId = null)
        {
            throw new NotImplementedException();
        }
    }
}
