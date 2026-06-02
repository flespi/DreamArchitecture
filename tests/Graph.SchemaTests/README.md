# Graph Schema Tests

Tests for the **Graph** layer, focused on validating the GraphQL schema.

## Responsibilities
- Execute GraphQL queries and mutations
- Validate operation results
- Verify resolver behavior
- Ensure correct mapping to Application use cases

## Dependencies
- Depends on **Graph.Schema** and **Graph.DataLoaders**
- Depends on **Application** and **Infrastructure** indirectly
- Uses external testing libraries

## Notes
These tests validate the GraphQL schema, executing operations and comparing the results against snapshots.
