using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace TimMovie.SharedKernel.Cache;

internal class CompiledExpressions<TIn, TOut>
{
    private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache = new();

    internal static Func<TIn, TOut> AsFunc(Expression<Func<TIn, TOut>> expression)
    {
        return Cache.GetOrAdd(expression, func => func.Compile());
    } 
}