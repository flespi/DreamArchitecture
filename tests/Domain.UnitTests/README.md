# Domain Unit Tests

Tests for the **Domain** layer, focused on validating core business rules and domain logic in isolation.

## Responsibilities
- Validate entities behavior
- Validate value objects
- Verify business rules
- Ensure invariants are enforced

## Dependencies
- Depends only on **Domain**
- Must not depend on Application, Infrastructure, or Graph
- Uses external testing libraries

## Notes
Tests should focus on pure domain logic without introducing external concerns or abstractions from other layers.
