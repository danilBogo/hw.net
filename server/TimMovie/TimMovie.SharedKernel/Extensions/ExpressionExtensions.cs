using System.Linq.Expressions;
using TimMovie.SharedKernel.Cache;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.SharedKernel.Extensions;

public static class ExpressionExtensions
{
    public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expression)
    {
        ArgumentValidator.ThrowExceptionIfNull(expression, nameof(expression));

        return CompiledExpressions<TIn, TOut>.AsFunc(expression);
    }
}