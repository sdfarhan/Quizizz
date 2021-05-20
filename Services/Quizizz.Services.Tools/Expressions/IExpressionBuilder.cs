using System;
using System.Linq.Expressions;

namespace Quizizz.Services.Tools.Expressions
{
    public interface IExpressionBuilder
    {
        Expression<Func<T, bool>> GetExpression<T>(string queryType, string queryValue, string roleId = null);
    }
}