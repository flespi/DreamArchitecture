# Graph.Schema Layer

The **Graph.Schema** project defines the GraphQL contract of the application.

## Responsibilities
- Query and Mutation definitions
- Schema types and field resolvers
- Authorization rules
- Interceptors and error handling

## Dependencies
- Depends only on **Application**
- Must not depend on Infrastructure

## Notes
This layer defines how clients interact with the system through GraphQL without containing application or data access logic.