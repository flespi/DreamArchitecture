using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.HasGuidKey(e => e.Id);

        builder.HasConcurrencyToken();

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(e => e.Colour);

        builder.OwnsOne(e => e.Audit);
    }
}
