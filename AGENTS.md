# AGENTS.md

Guidelines for working with this codebase.

---

## 1. Technologies

- ASP.NET Core
- Entity Framework Core
- HotChocolate, GreenDonut, StrawberryShake
- MediatR
- FluentValidation
- xUnit, Shouldly, Moq, Respawn
- Testcontainers
- Orca

---

## 2. Architecture

### Source Projects (`src/`)

| Project | Responsibilities | Dependencies |
|-------|-----------------|--------------|
| **Domain** | Entities, Value Objects, Aggregates, Domain Events, Business rules/invariants | None |
| **Application** | CQRS Commands/Queries, Handlers, Validators, Pipeline Behaviors, Event Handlers | Domain |
| **Infrastructure** | EF Core DbContext, Repositories, Identity/Auth, External services, Background services | Domain, Application |
| **Graph.Schema** | GraphQL Queries/Mutations, Schema types, Field resolvers, Authorization | Application |
| **Graph.DataLoaders** | DataLoader implementations, Request-scoped batching/caching | Application, Infrastructure |
| **Graph** | GraphQL endpoint, DI setup, Entry point | All components |

### Test Projects (`tests/`)

| Project | What to Test |
|-------|--------------|
| **Domain.UnitTests** | Entities, Value Objects, Business rules, Invariants |
| **Application.UnitTests** | Component behavior, Validation rules, Pipeline behaviors |
| **Application.FunctionalTests** | Commands/Queries execution, Handler validation |
| **Graph.FunctionalTests** | GraphQL queries/mutations, Operation results |
| **Graph.SchemaTests** | GraphQL queries/mutations, Operation results |
| **Infrastructure.IntegrationTests** | Data persistence, External service integrations |

---

## 3. Domain Guidelines

### Entity Base Classes

| Class | Purpose |
|-------|---------|
| `IAggregateRoot` | Base marker for aggregates |
| `BaseAggregateRoot` | Provides Id, Audit, ConcurrencyToken, DomainEvents |
| `IBaseEntity<T>` | Base entity with Id |
| `IConcurrentEntity` | Optimistic concurrency token |
| `IAuditableEntity` | Audit trail properties |
| `IDomainEventCollector` | Domain event collection |

### Creating an Entity

```csharp
public class TodoList : BaseAggregateRoot
{
    public required string Title { get; set; }
    public Colour? Colour { get; set; }
    public List<TodoItem> Items { get; set; } = [];
}
```

### Domain Events

Create in `src/Domain/Events/`:

```csharp
public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItemCreatedEvent(TodoItem item) => Item = item;
    public TodoItem Item { get; }
}
```

---

## 4. Application Guidelines

### Folder Structure

```
src/Application/<Feature>/
├── Commands/<Action>/
│   ├── <Action>Command.cs
│   ├── <Action>Data.cs
│   └── <Action>CommandValidator.cs
├── Queries/<Action>/
│   ├── <Action>Query.cs
│   └── <Action>QueryHandler.cs
├── Validators/
│   └── InlineValidators.cs
└── EventHandlers/
```

### Command Pattern

```csharp
[Idempotent]
[Transactional]
public record CreateTodoListCommand : IRequest<TodoList>
{
    public required CreateTodoListData Data { get; init; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, TodoList>
{
    private readonly IUnitOfWork _uow;

    public async Task<TodoList> Handle(CreateTodoListCommand request, CancellationToken ct)
    {
        var entity = new TodoList { Title = request.Data.Title };
        await _uow.TodoList.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity;
    }
}
```

### Attributes

| Attribute | Purpose |
|-----------|---------|
| `[Idempotent]` | Prevents duplicate execution via idempotency key |
| `[Transactional]` | Wraps in DB transaction |
| `[Condition]` | Optimistic concurrency |
| `[Authorize]` | Requires authorization |

### Update Data Pattern

Use `Optional<T>` for nullable updates:

```csharp
public record UpdateTodoListData
{
    public Optional<string> Title { get; init; }
    public Optional<Colour?> Colour { get; init; }
}
```

### Inline Validators

Validation rules are defined as reusable `InlineValidator` classes:

```csharp
public static class TodoListRuleSet
{
    public static InlineValidator<string?> Title { get; } = [];

    static TodoListRuleSet()
    {
        Title.RuleFor(x => x).NotEmpty().MaximumLength(200);
    }
}
```

Use the rule set in command validators:

```csharp
public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        Include(TodoListRuleSet.Title);

        RuleFor(v => v.Data.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
```

### Unit of Work

Add repositories to `IUnitOfWork`:

```csharp
IRepository<TodoList> TodoList => Repository<TodoList>();
```

---

## 5. Infrastructure Guidelines

### Entity Configuration

```csharp
public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.HasGuidKey(e => e.Id);
        builder.HasConcurrencyToken();
        builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
        builder.OwnsOne(e => e.Colour);
        builder.OwnsOne(e => e.Audit);
        builder.Navigation(e => e.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
```

### Aggregate Boundaries

For entities with owned entities, define boundaries:

```csharp
public static class TodoListAggregate
{
    public static IAggregateBoundary<TodoList> Boundary { get; } = new AggregateBoundary<TodoList>()
        .Include(x => x.Items);
}
```

Register in DI:

```csharp
services.AddSingleton(TodoListAggregate.Boundary);
```

### Migrations

To add a migration:

```powershell
dotnet ef migrations add <Name> --project src/Infrastructure/Infrastructure.csproj
```

To update the database:

```powershell
dotnet ef database update --project src/Infrastructure/Infrastructure.csproj -- -c <connection_string>
```

### Seeders

Create in `src/Infrastructure/Data/Seeders/`:

```csharp
[DbContext(typeof(ApplicationDbContext))]
[DataSeeder("00000000010000_InitialSeed")]
public class InitialSeed : IDataSeeder<ApplicationDbContext>
{
    private readonly ILogger<InitialSeed> _logger;

    public InitialSeed(ILogger<InitialSeed> logger) => _logger = logger;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken ct)
    {
        context.TodoLists.Add(new TodoList { Title = "Todo List" });
        await context.SaveChangesAsync(ct);
    }
}
```

Naming: `<timestamp>_<Name>Seed.cs` (earliest runs first)

---

## 6. GraphQL Schema Guidelines

> **Important:** GraphQL schema files go in `src/Graph.Schema/`, NOT in `src/Graph/`. The Graph project is the runtime host only.

### Object Types

Define in `src/Graph.Schema/Schema/<Entity>/Objects/`:

```csharp
public class TodoListType : ObjectType<TodoList>
{
    protected override void Configure(IObjectTypeDescriptor<TodoList> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.Title);
        descriptor.Field(x => x.Colour).Type<ColourType>();
        descriptor.Field(x => x.Items)
            .ResolveWith<TodoListResolvers>(x => x.GetTodoItemAsync(default!, default!, default!, default));
    }
}
```

### Filter Input Types

In `src/Graph.Schema/Schema/<Entity>/FilterInputs/`:

```csharp
public class TodoListFilterType : FilterInputType<TodoList>
{
    protected override void Configure(IFilterInputTypeDescriptor<TodoList> descriptor)
    {
        descriptor.Field(t => t.Id);
        descriptor.Field(t => t.Title);
    }
}
```

### Sort Input Types

In `src/Graph.Schema/Schema/<Entity>/SortInputs/`:

```csharp
public class TodoListSortType : SortInputType<TodoList>
{
    protected override void Configure(ISortInputTypeDescriptor<TodoList> descriptor)
    {
        descriptor.Field(t => t.Title);
    }
}
```

---

## 7. GraphQL Operations Guidelines

> **Important:** GraphQL operation files go in `src/Graph.Schema/`, NOT in `src/Graph/`. The Graph project is the runtime host only.

### Queries

In `src/Graph.Schema/Operations/Queries/<Feature>Query.cs`:

```csharp
[Authorize]
[QueryType]
public static class TodoListQuery
{
    [NodeResolver]
    public static async Task<TodoList?> GetTodoListById(
        Guid id,
        ISelection selection,
        ITodoListByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
        => await dataLoader.Select(selection).LoadAsync(id, cancellationToken);

    [UsePaging]
    [UseFiltering(typeof(TodoListFilterType))]
    [UseSorting(typeof(TodoListSortType))]
    public static async Task<Connection<TodoList>> GetTodoLists(
        PagingArguments pagingArgs,
        QueryContext<TodoList> query,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var request = new GetTodosQuery { PagingArgs = pagingArgs, Specification = query };
        return await sender.Send(request, cancellationToken).ToConnectionAsync();
    }
}
```

### Mutations

In `src/Graph.Schema/Operations/Mutations/<Feature>Mutation.cs`:

```csharp
[Authorize]
[MutationType]
public static class TodoListMutation
{
    public static async Task<TodoList> CreateTodoList(CreateTodoListData data, [Service] ISender sender)
        => await sender.Send(new CreateTodoListCommand { Data = data });

    public static async Task<TodoList> UpdateTodoList([ID] Guid id, UpdateTodoListData data, Condition? condition, [Service] ISender sender)
        => await sender.Send(new UpdateTodoListCommand { Id = id, Data = data, Condition = condition });

    public static async Task<TodoList> DeleteTodoList([ID] Guid id, [Service] ISender sender)
        => await sender.Send(new DeleteTodoListCommand { Id = id });
}
```

---

## 8. DataLoader Guidelines

> **Important:** GraphQL DataLoader files go in `src/Graph.DataLoaders/`, NOT in `src/Graph/`. The Graph project is the runtime host only.

### Define Interface (Application)

```csharp
public interface ITodoListByIdDataLoader : IDataLoader<Guid, TodoList> { }
```

### Implement DataLoader (Graph.DataLoaders)

```csharp
public partial class TodoListByIdDataLoader : ITodoListByIdDataLoader
{
    [DataLoader("TodoListById")]
    public static async Task<IReadOnlyDictionary<Guid, TodoList>> FetchAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<TodoList> query,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
        => await context.TodoLists
            .Where(t => ids.Contains(t.Id))
            .With(query)
            .ToDictionaryAsync(g => g.Id, cancellationToken);
}
```

### Register in DI

```csharp
services.AddDataLoader<ITodoListByIdDataLoader, TodoListByIdDataLoader>();
```

---

## 9. Testing Guidelines

### Domain Unit Tests

```csharp
public class ColourTests
{
    [Fact]
    public void ShouldReturnCorrectColourCode()
    {
        var colour = Colour.From("#FFFFFF");
        colour.Code.ShouldBe("#FFFFFF");
    }
}
```

### Application Unit Tests

```csharp
public class ValidationExceptionTests
{
    [Fact]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        new ValidationException().Errors.Keys.ShouldBeEmpty();
    }
}
```

### Application Functional Tests

```csharp
public class CreateTodoListTests : BaseTest
{
    public CreateTodoListTests(AppTestContext context) : base(context) { }

    [Fact]
    public async Task ShouldCreateTodoList()
    {
        var userId = await RunAsDefaultUserAsync();
        var list = await SendAsync(new CreateTodoListCommand { Data = new() { Title = "Tasks" } });
        var result = await FindAsync<TodoList>(list.Id);

        result.ShouldNotBeNull();
        result!.Title.ShouldBe("Tasks");
        result.Audit.CreatedBy.ShouldBe(userId);
    }
}
```

### Graph Functional Tests

```csharp
public class GetTodosTests : BaseTest
{
    public GetTodosTests(AppTestContext context) : base(context) { }

    [Fact]
    public async Task ShouldReturnAllLists()
    {
        await RunAsDefaultUserAsync();
        var response = await Client.GetTodoLists.ExecuteAsync(TestContext.Current.CancellationToken);
        response.IsSuccessResult().ShouldBeTrue();
    }
}
```

### Graph Schema Tests

```yaml
name: CreateTodoList

document:
  kind: inline
  spec: |
    mutation CreateTodoList($input: CreateTodoListInput!) {
      createTodoList(input: $input) {
        todoList {
          title
        }
      }
    }

identity:
  sub: test
  name: test

variables:
  input:
    data:
      title: "Shoping"

snapshot:
  kind: inline
  spec:
    data:
      createTodoList:
        todoList:
          title: "Shoping"
```

### Base Class Helpers

| Method | Purpose |
|--------|---------|
| `SendAsync<TResponse>(IRequest<TResponse>)` | Send command/query via MediatR |
| `FindAsync<TEntity>(keyValues)` | Find entity in DB |
| `RunAsDefaultUserAsync()` | Set default user identity |
| `RunAsAdministratorAsync()` | Set admin identity |
| `Should.ThrowAsync<T>()` | Shouldly assertion |

---

## 10. Localization Guidelines

### Create Resource File

Create in `src/Application/Resources/ValidationMessages.resx`:

| Name | Value |
|------|-------|
| `Unique_Field` | The {0} must be unique. |

### Use in Validator

```csharp
public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator(IUnitOfWork uow, IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(v => v.Data.Title)
            .MustAsync(BeUniqueTitle)
                .WithMessage(localizer["Unique_Field"])
                .WithErrorCode("Unique");

        async Task<bool> BeUniqueTitle(CreateTodoListCommand request, string? title, CancellationToken ct)
            => !await uow.TodoList.AnyAsync(t => t.Title == title, ct);
    }
}
```

---

## 11. Development Commands

```powershell
# Restore tools
dotnet tool restore

# Build
dotnet build

# Run
dotnet run --project src/Graph

# Test
dotnet test

# Generate GraphQL client code
dotnet graphql generate

# EF Migrations
dotnet ef migrations add <Name> --project src/Infrastructure/Infrastructure.csproj
```
