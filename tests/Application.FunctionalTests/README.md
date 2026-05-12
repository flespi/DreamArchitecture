# Application Functional Tests

Tests for the **Application** layer, focused on validating use cases through Commands and Queries.

## Responsibilities
- Execute Commands and Queries
- Validate handlers behavior
- Verify use case validation
- Ensure pipeline behaviors are applied

## Dependencies
- Depends on **Graph** only as an entry point
- Depends on **Application** and **Domain** indirectly
- Uses external testing libraries

## Notes
These tests validate use cases at the Application level, involving multiple components working together. They do not involve API concerns.
