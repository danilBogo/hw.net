using System.Linq.Expressions;
using TimMovie.SharedKernel.Extensions;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.SharedKernel.Specification;

public class Specification<TEntity>: ISpecification<TEntity>
{
    protected Expression<Func<TEntity, bool>> Conditional = null!;
    
    protected Specification(){}

    public Specification(Expression<Func<TEntity, bool>> conditional)
    {
        ArgumentValidator.ThrowExceptionIfNull(conditional, nameof(conditional));
        
        Conditional = conditional;
    }

    public bool IsSatisfiedBy(TEntity entity)
    {
        ArgumentValidator.ThrowExceptionIfNull(entity!, nameof(entity));

        if (Conditional is null)
        {
            throw new InvalidOperationException($"{nameof(Conditional)} равен null.");
        }
        
        return Conditional.AsFunc()(entity);
    }

    public Expression<Func<TEntity, bool>> ToExpression()
    {
        return Conditional;
    }

    public static bool operator true(Specification<TEntity> _)
    {
        return false;
    }

    public static bool operator false(Specification<TEntity> _)
    {
        return false;
    }

    public static Specification<TEntity> operator &(Specification<TEntity> spec1, Specification<TEntity> spec2)
    {
        CheckSpecificationsOnNull(spec1, spec2);
        
        var exp1 = spec1.Conditional;
        var exp2 = spec2.Conditional;

        var newLambda = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(exp1.Body, Expression.Invoke(exp2, exp1.Parameters)),
            exp1.Parameters);
        
        return new Specification<TEntity>(newLambda);
    }
    
    public static Specification<TEntity> operator |(Specification<TEntity> spec1, Specification<TEntity> spec2)
    {
        CheckSpecificationsOnNull(spec1, spec2);
        
        var exp1 = spec1.Conditional;
        var exp2 = spec2.Conditional;

        var newLambda = Expression.Lambda<Func<TEntity, bool>>(
            Expression.OrElse(exp1.Body, Expression.Invoke(exp2, exp1.Parameters)),
            exp1.Parameters);
        
        return new Specification<TEntity>(newLambda);
    }

    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> spec)
    {
        ArgumentValidator.ThrowExceptionIfNull(spec, nameof(spec));
        
        return spec.Conditional;
    }

    private static void CheckSpecificationsOnNull(Specification<TEntity> spec1, Specification<TEntity> spec2)
    {
        ArgumentValidator.ThrowExceptionIfNull(spec1, nameof(spec1));
        ArgumentValidator.ThrowExceptionIfNull(spec2, nameof(spec2));
    }
}