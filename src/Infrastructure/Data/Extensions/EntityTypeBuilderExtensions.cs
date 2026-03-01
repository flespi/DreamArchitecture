using System.Linq.Expressions;
using CleanArchitecture.Domain.Common;
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

    public static EntityTypeBuilder<TEntity> HasConcurrencyToken<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IConcurrentEntity
    {
        builder.Property(x => x.ConcurrencyToken)
            .HasConversion<HexToBytesConverter>()
            .IsRowVersion();

        return builder;
    }
}
