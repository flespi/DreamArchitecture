using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CleanArchitecture.Infrastructure.Data;

public static class DbContextExtensions
{
    public static object?[] GetKeyValues<TEntity>(this DbContext context, TEntity entity)
        where TEntity : class
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var keyProps = GetPrimaryKeyProperties<TEntity>(context);

        var entry = context.Entry(entity);
        var values = new object?[keyProps.Count];

        for (int i = 0; i < keyProps.Count; i++)
        {
            var prop = keyProps[i];
            if (prop.PropertyInfo != null)
            {
                values[i] = prop.PropertyInfo.GetValue(entity);
            }
            else
            {
                // shadow property: use EF Core entry to access current value
                values[i] = entry.Property(prop.Name).CurrentValue;
            }
        }

        return values!;
    }

    public static Expression<Func<TEntity, bool>> CreateGetByKeyPredicate<TEntity>(
        this DbContext context,
        params object[]? keyValues)
        where TEntity : class
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (keyValues == null) throw new ArgumentNullException(nameof(keyValues));

        var keyProps = GetPrimaryKeyProperties<TEntity>(context);

        if (keyValues.Length != keyProps.Count)
            throw new ArgumentException($"Expected {keyProps.Count} key value(s) for entity '{typeof(TEntity).Name}' but received {keyValues.Length}.", nameof(keyValues));

        var parameter = Expression.Parameter(typeof(TEntity), "e");
        Expression? body = null;

        for (int i = 0; i < keyProps.Count; i++)
        {
            var prop = keyProps[i];
            var equality = BuildEqualityExpression(parameter, prop, keyValues[i]);
            body = body == null ? equality : Expression.AndAlso(body, equality);
        }

        if (body == null)
            throw new InvalidOperationException("Failed to build key predicate.");

        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }

    private static IReadOnlyList<IProperty> GetPrimaryKeyProperties<TEntity>(DbContext context)
        where TEntity : class
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity))
            ?? throw new InvalidOperationException($"Entity type '{typeof(TEntity)}' is not part of the DbContext model.");

        var primaryKey = entityType.FindPrimaryKey()
            ?? throw new InvalidOperationException($"Entity '{entityType.Name}' does not have a primary key defined.");

        if (primaryKey.Properties.Count == 0)
            throw new InvalidOperationException($"Entity '{entityType.Name}' primary key contains no properties.");

        return primaryKey.Properties;
    }

    private static Expression BuildEqualityExpression(ParameterExpression parameter, IProperty prop, object? rawValue)
    {
        // Build left side: either CLR property access or EF.Property<T>(e, "Name") for shadow properties
        Expression left = GetPropertyAccessExpression(parameter, prop);

        // Convert the raw value to the target CLR type
        var targetType = prop.PropertyInfo?.PropertyType ?? prop.ClrType;

        Expression right = Expression.Constant(rawValue, targetType);

        // If left is a value type but right might be boxed, ensure both sides have same exact type for Expression.Equal
        if (left.Type != right.Type)
        {
            if (left.Type.IsAssignableFrom(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (right.Type.IsAssignableFrom(left.Type))
                left = Expression.Convert(left, right.Type);
            else
            {
                // last resort: convert right to left.Type
                right = Expression.Convert(right, left.Type);
            }
        }

        return Expression.Equal(left, right);
    }

    private static Expression GetPropertyAccessExpression(ParameterExpression parameter, IProperty prop)
    {
        if (prop.PropertyInfo != null)
        {
            return Expression.Property(parameter, prop.PropertyInfo);
        }

        // Shadow property -> EF.Property<T>(e, "PropertyName")
        var efPropertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(prop.ClrType);

        return Expression.Call(efPropertyMethod, parameter, Expression.Constant(prop.Name));
    }
}
