# Graph Layer

The **Graph** project hosts the GraphQL endpoint of the application.

## Responsibilities
- GraphQL endpoint exposure
- Register Schema and DataLoaders
- Middleware and filters
- Dependency injection setup for all layers

## Dependencies
- Depends on **Graph.Schema** and **Graph.DataLoaders**
- Can depend on **Application** and **Infrastructure** indirectly

## Notes
This is the entry point of the application at runtime and composes all layers together.