# Mini Asset Management System

This project is a demonstration of clean architecture principles and ASP.NET
implementation. It focuses on managing assets with role-based access control
(RBAC). Unlike traditional user roles, asset roles specifically define user
access permissions to specific assets. This project explores a novel approach
for storing these permissions in a database, aiming to provide a
proof-of-concept for future advancements in asset management systems like
Google Drive or OneDrive.

## Features

This project establishes a modular system with distinct domains: User, Drive,
and Asset. Each domain encapsulates its specific business logic, minimizing
interdependencies.

Domain Breakdown:

- User Domain: User creation, update, deletion, retrieval, and listing.
- Drive Domain: Drive creation, renaming, deletion, retrieval, and listing.
- Asset Domain:
  - Folder and file asset creation, update, deletion, retrieval,
    and listing.
  - Add, update, and remove user permissions for assets.

## Architecture

<img
  src="https://github.com/user-attachments/assets/06143c4f-c42f-4008-93a1-e198ece94554"
  alt="Mini Asset Management Architecture" width="400" height="400" />

This project employs a Clean Architecture approach, organized into distinct
layers to enhance maintainability and testability.

**Core Project**:

- Serves as the central hub of the application, defining the core domain model,
  including entities, value objects, events, and specifications.
- Minimizes external dependencies to ensure a focused business logic layer.

**Use Cases Project**:

- Implements application services organized using the Command Query
  Responsibility Segregation (CQRS) pattern.
- Commands modify the domain model and utilize repository abstractions for data access.
- Queries are read-only and may employ alternative data access strategies.
- Leverages specifications to encapsulate query logic and result type mapping.

**Infrastructure Project**:

- Houses external dependencies and infrastructure-specific implementations.
- Adheres to interfaces defined in the Core project to maintain a clear
  separation of concerns.

**Web Project**:

- Serves as the entry point for the application, handling API endpoints using
  the FastEndpoints library.

**Key Advantages of Clean Architecture**:

- Improved Maintainability: Enhances code organization and reduces coupling.
- Testability: Facilitates unit testing of business logic independently.
- Flexibility: Enables easier adaptation to changes and technology updates.
- Scalability: Supports the growth and evolution of the application.

## Implementation

### Entity Relationship Diagram

![Mini Asset Management ERD](https://github.com/user-attachments/assets/ea0a5297-a9d5-4a05-8ab2-74f7c2810515)

User Entity:

- Excludes authentication information to maintain a clear focus on asset
  management logic.

Asset Entity:

- Represents both files and folders within the system.
- Differentiates between file and folder types using a `FileType` field (null
  for folders).

Data Omission:

- To simplify the initial implementation, the User, Drive, Permission and Asset
  entities do not currently include `CreatedAt` and `UpdatedAt` timestamps. These
  fields can be added in future iterations if deemed necessary.

## Testing

This solution utilizes separate test projects for unit, functional, and
integration tests, promoting organization and maintainability.

Testing Dependencies:

- [NUnit](https://www.nuget.org/packages/NUnit/): The primary test framework
  chosen for its versatility and extensive features.
- [NSubstitute](https://www.nuget.org/packages/NSubstitute): Employed as a
  mocking framework for white-box testing scenarios requiring simulation of
  dependencies. It provides a lightweight and convenient approach compared to
  custom mock implementations.
- [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost):
  Leverages TestHost to conduct full-stack testing of the web project. This
  allows for executing HttpClient requests within memory, streamlining testing
  by eliminating external dependencies like firewalls or specific port
  configurations. Additionally, it promotes faster test execution times by
  running tests entirely in memory.
