# Dream Architecture Solution Template

[![Build](https://github.com/flespi/DreamArchitecture/actions/workflows/build.yml/badge.svg)](https://github.com/flespi/DreamArchitecture/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Dream.Architecture.Solution.Template?label=NuGet)](https://www.nuget.org/packages/Dream.Architecture.Solution.Template)
[![Downloads](https://img.shields.io/nuget/dt/Dream.Architecture.Solution.Template?label=Downloads)](https://www.nuget.org/packages/Dream.Architecture.Solution.Template)

This template combines elements of Clean Architecture and Domain-Driven Design (DDD). The solution defines explicit boundaries between domain, application, and infrastructure. GraphQL serves as the API to expose application functionality, supporting a CQRS approach and a unified response schema. The template provides a consistent structure for enterprise .NET applications.

## Getting Started

Install the template from Nuget:

```bash
dotnet new install Dream.Architecture.Solution.Template
```

Once installed, create a new solution using the template.

```bash
dotnet new da-sln -o YourProjectName
```

## Technologies

- ASP.NET Core
- Entity Framework Core
- HotChocolate, GreenDonut, StrawberryShake
- MediatR
- FluentValidation
- xUnit, Shouldly, Moq & Respawn
- Testcontainers
- Orca

## Thanks

### Jason Taylor ([jasontaylordev](https://github.com/jasontaylordev))

This template started as a fork of his. For a long time, I embraced it as a reference architecture. It has proven to be an exceptional example of how to implement clean architecture in a simple way.

### Steve Smith ([ardalis](https://github.com/ardalis))

Many of his contributions have been useful to me in revisiting DDD and redefining the worth of aggregates. The repository implementantion of this template is directly inspired by his *Specification* package.

### Michael Staib ([michaelstaib](https://github.com/michaelstaib))

He is not just the promoter of the most popular GraphQL implementation for .NET, but also of hundreds of tutorials that have facilitated its adoption.

### Third-Party Contributors

A sincere thanks to all third-party contributors whose dedication and hard work make the packages used in this template possible. Their contributions streamline development and strengthen the developer community. Every effort to maintain, improve, and share these tools is greatly appreciated.

## License

This project is licensed with the MIT license.
