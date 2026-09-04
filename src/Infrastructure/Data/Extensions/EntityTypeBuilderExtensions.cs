using System.Linq.Expressions;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Data.Configurations;
using CleanArchitecture.Infrastructure.Data.ValueConversion;
using CleanArchitecture.Infrastructure.Data.ValueGeneration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasGuidKey<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> keyExpression)
        where TEntity : class
    {
        builder.HasKey(keyExpression);

        builder.Property(keyExpression)
            .HasConversion<GuidToBytesConverter>()
            .HasValueGenerator<ChronologicalGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        return builder;
    }

    public static EntityTypeBuilder<TEntity> HasConcurrencyToken<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TProperty>> propertyExpression)
        where TEntity : class, IConcurrentEntity
    {
        builder.Property(propertyExpression)
            .HasConversion<HexToBytesConverter>()
            .IsRowVersion();

        return builder;
    }

    public static EntityTypeBuilder<TEntity> OwnsOne<TEntity, TRelatedEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TRelatedEntity?>> navigationExpression,
        IOwnedNavigationConfiguration<TEntity, TRelatedEntity> navigationConfiguration)
        where TEntity : class
        where TRelatedEntity : class
    {
        return builder.OwnsOne(navigationExpression, b => navigationConfiguration.Configure(b));
    }
}
