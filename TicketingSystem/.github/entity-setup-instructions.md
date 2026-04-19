# Entity Setup Instructions

This document provides step-by-step instructions for adding new entities to the TicketingSystem using the established architectural patterns.

## Architecture Overview

The TicketingSystem follows a Clean Architecture/Layered Architecture pattern with the following key projects:

- **TicketingSystem.Domain** - Contains domain models, interfaces, and business logic
- **TicketingSystem.DataAccess** - Contains EF Core entities, configurations, repositories, and database context
- **TicketingSystem.Common** - Contains shared utilities and extensions

## Entity Implementation Pattern

When adding a new entity, follow these steps in order:

---

## Step 1: Define Domain Model (TicketingSystem.Domain)

**Location:** `src/TicketingSystem.Domain/Models/{EntityName}.cs`

**Template:**
```csharp
namespace TicketingSystem.Domain.Models;

public sealed record {EntityName} : DomainModelBase
{
    // Add entity-specific properties here
    // Do NOT include Id, CreatedAt, LastModifiedAt - they come from DomainModelBase
}
```

**Base Class:**
- **DomainModelBase** - Abstract base class for all domain models
  - Provides: `Id` (Guid), `CreatedAt` (DateTimeOffset), `LastModifiedAt` (DateTimeOffset?)

**Naming Convention:**
- Use singular noun (e.g., `User`, `Customer`, `Event`)
- Use `sealed record` for immutability
- Inherit from `DomainModelBase`
- Properties should use PascalCase

**Example (User):**
```csharp
namespace TicketingSystem.Domain.Models;

public sealed record User : DomainModelBase
{
    public UserType Type { get; set; }
    public required string Email { get; set; }
}
```

---

## Step 2: Define Repository Interfaces (TicketingSystem.Domain)

**Location:** `src/TicketingSystem.Domain/Interfaces/Repositories/`

### 2.1 Read Repository Interface

**File:** `I{EntityName}ReadRepository.cs`

**Template:**
```csharp
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface I{EntityName}ReadRepository
{
    Task<{EntityName}?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    // Add entity-specific read methods
}
```

**Example (User):**
```csharp
public interface IUserReadRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
```

### 2.2 Write Repository Interface

**File:** `I{EntityName}WriteRepository.cs`

**Template:**
```csharp
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface I{EntityName}WriteRepository
{
    /// <summary>
    /// Adds a new {entity} to the repository.
    /// </summary>
    /// <param name="{entity}Model">The {entity} model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added {entity}.</returns>
    Task<Guid> AddAsync({EntityName} {entity}Model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing {entity}.
    /// </summary>
    /// <param name="{entity}Model">The {entity} model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the {entity}
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync({EntityName} {entity}Model, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the {entity} with the specified unique identifier.
    /// </summary>
    /// <param name="{entity}Id">The unique identifier of the {entity} to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the {entity}
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid {entity}Id, CancellationToken cancellationToken);
}
```

---

## Step 3: Create Entity (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/Entities/{EntityName}Entity.cs`

**Template:**
```csharp
using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class {EntityName}Entity : AuditableEntityBase
{
    // Add entity-specific properties here
    // Do NOT include Id, CreatedAt, LastModifiedAt, IsDeleted - they come from base classes
}
```

**Base Classes:**
- **AuditableEntityBase** - For entities that need audit tracking (CreatedAt, LastModifiedAt)
  - Inherits from: `DbEntityBase` (provides Id, IsDeleted)
  - Implements: `IAuditableEntity`
- **DbEntityBase** - For simple entities without audit tracking (only Id, IsDeleted)

**Example (User):**
```csharp
internal sealed class UserEntity : AuditableEntityBase
{
    public UserType Type { get; set; }
    public required string Email { get; set; }
}
```

**Important Notes:**
- Entities are `internal` (not public)
- Use `sealed` class
- Properties inherited from base:
  - `Id` (Guid) - from DbEntityBase
  - `IsDeleted` (bool) - from DbEntityBase
  - `CreatedAt` (DateTimeOffset) - from AuditableEntityBase
  - `LastModifiedAt` (DateTimeOffset?) - from AuditableEntityBase

---

## Step 4: Create Entity Configuration (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/EntityConfigurations/{EntityName}EntityConfiguration.cs`

**Template:**
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class {EntityName}EntityConfiguration : IEntityTypeConfiguration<{EntityName}Entity>
{
    public void Configure(EntityTypeBuilder<{EntityName}Entity> builder)
    {
        builder.ToTable("{TableName}"); // Usually plural: "Users", "Customers"

        builder.HasKey(x => x.Id);

        // Configure properties
        builder
            .Property(x => x.{PropertyName})
            .IsRequired()
            .HasMaxLength(DbConstants.{ConstantName});

        // Configure indexes (if needed)
        builder
            .HasIndex(x => x.{PropertyName})
            .IsUnique()
            .HasFilter("[IsDeleted] = 0"); // For soft-delete support

        // Configure relationships (if needed)
        builder
            .HasOne(x => x.{NavigationProperty})
            .WithOne()
            .HasForeignKey<{EntityName}Entity>(x => x.{ForeignKeyProperty})
            .OnDelete(DeleteBehavior.NoAction);
    }
}
```

**Example (User):**
```csharp
internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(DbConstants.MaxEmailLength);

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
```

**Common Patterns:**
- **String properties:** Always set `IsRequired()` and `HasMaxLength()`
- **Unique indexes:** Include `.HasFilter("[IsDeleted] = 0")` for soft-delete support
- **Foreign keys:** Use `OnDelete(DeleteBehavior.NoAction)` to prevent cascade deletes
- **Constants:** Add length constants to `DbConstants.cs` (e.g., `MaxEmailLength = 255`, `ShortTextMaxLength = 255`)

---

## Step 5: Add DbSet to DbContext (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/TicketingDbContext.cs`

**Action:** Add internal DbSet property

**Pattern:**
```csharp
internal DbSet<{EntityName}Entity> {EntityNamePlural} => Set<{EntityName}Entity>();
```

**Example:**
```csharp
internal DbSet<UserEntity> Users => Set<UserEntity>();
```

**Note:** Entity configurations are auto-discovered via `ApplyConfigurationsFromAssembly()` - no manual registration needed.

---

## Step 6: Create Mapper (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/Mappers/{EntityName}Mapper.cs`

**Template:**
```csharp
using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class {EntityName}Mapper
{
    [MapperIgnoreSource(nameof({EntityName}Entity.IsDeleted))]
    public static partial {EntityName} FromEntity({EntityName}Entity {entity}Entity);

    [MapperIgnoreTarget(nameof({EntityName}Entity.IsDeleted))]
    public static partial {EntityName}Entity ToEntity({EntityName} {entity}Model);
}
```

**Example (User):**
```csharp
[Mapper]
internal static partial class UserMapper
{
    [MapperIgnoreSource(nameof(UserEntity.IsDeleted))]
    public static partial User FromEntity(UserEntity userEntity);

    [MapperIgnoreTarget(nameof(UserEntity.IsDeleted))]
    public static partial UserEntity ToEntity(User userModel);
}
```

**Important Notes:**
- Uses **Riok.Mapperly** (compile-time source generator)
- Always ignore `IsDeleted` property (internal to data layer)
- Mapper methods are `partial` - implementation is auto-generated
- Use lowercase parameter names (e.g., `userEntity`, `userModel`)

---

## Step 7: Implement Read Repository (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/Repositories/{EntityNamePlural}/{EntityName}ReadRepository.cs`

**Template:**
```csharp
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.{EntityNamePlural};

internal sealed class {EntityName}ReadRepository : I{EntityName}ReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<{EntityName}Entity> Active{EntityNamePlural} 
        => _dbContext.{EntityNamePlural}.Where(x => !x.IsDeleted);

    public {EntityName}ReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<{EntityName}?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var {entity}Entity = await Active{EntityNamePlural}
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var {entity}Model = {entity}Entity.MapIfNotNull({EntityName}Mapper.FromEntity);
        return {entity}Model;
    }

    // Implement other read methods
}
```

**Example (User):**
```csharp
internal sealed class UserReadRepository : IUserReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<UserEntity> ActiveUsers 
        => _dbContext.Users.Where(x => !x.IsDeleted);

    public UserReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEntity = await ActiveUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
        return userModel;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEntity = await ActiveUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
        return userModel;
    }
}
```

**Key Patterns:**
- Use `Active{EntityNamePlural}` property for reusable soft-delete filtering
- Always use `.AsNoTracking()` for read operations
- Use `MapIfNotNull()` extension for null-safe mapping
- Repository is `internal sealed`

---

## Step 8: Implement Write Repository (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/Repositories/{EntityNamePlural}/{EntityName}WriteRepository.cs`

**Template:**
```csharp
using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.{EntityNamePlural};

internal sealed class {EntityName}WriteRepository : I{EntityName}WriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public {EntityName}WriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync({EntityName} {entity}Model, CancellationToken cancellationToken)
    {
        var {entity}Entity = {EntityName}Mapper.ToEntity({entity}Model);

        {entity}Entity.Id = Guid.NewGuid();

        await _dbContext.{EntityNamePlural}.AddAsync({entity}Entity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return {entity}Entity.Id;
    }

    public async Task<bool> UpdateAsync({EntityName} {entity}Model, CancellationToken cancellationToken)
    {
        var {entity}Entity = await GetByIdAsync({entity}Model.Id, cancellationToken);

        if ({entity}Entity == null)
            return false;

        // Update only specific properties
        {entity}Entity.{Property} = {entity}Model.{Property};

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid {entity}Id, CancellationToken cancellationToken)
    {
        var {entity}Entity = await GetByIdAsync({entity}Id, cancellationToken);

        if ({entity}Entity == null)
            return false;

        {entity}Entity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<{EntityName}Entity?> GetByIdAsync(Guid {entity}Id, CancellationToken cancellationToken)
    {
        return await _dbContext.{EntityNamePlural}
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == {entity}Id, cancellationToken);
    }
}
```

**Example (User):**
```csharp
internal sealed class UserWriteRepository : IUserWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public UserWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(User userModel, CancellationToken cancellationToken)
    {
        var userEntity = UserMapper.ToEntity(userModel);
        userEntity.Id = Guid.NewGuid();
        await _dbContext.Users.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return userEntity.Id;
    }

    public async Task<bool> UpdateAsync(User userModel, CancellationToken cancellationToken)
    {
        var userEntity = await GetByIdAsync(userModel.Id, cancellationToken);
        if (userEntity == null)
            return false;

        userEntity.Type = userModel.Type;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = await GetByIdAsync(userId, cancellationToken);
        if (userEntity == null)
            return false;

        userEntity.IsDeleted = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}
```

**Key Patterns:**
- **Soft Delete:** Set `IsDeleted = true` instead of removing from database
- **Update:** Only update specific properties, not the entire entity
- **Private GetByIdAsync:** Helper method for internal repository use (returns entity with tracking)
- **Audit:** `CreatedAt` and `LastModifiedAt` are handled automatically by `TicketingDbContext.SaveChangesAsync()`

---

## Step 9: Register Repositories (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/Extensions/ServiceCollectionExtensions.cs`

**Action:** Add repository registrations to `AddRepositories` method

**Pattern:**
```csharp
public static IServiceCollection AddRepositories(this IServiceCollection services)
{
    services.AddScoped<I{EntityName}ReadRepository, {EntityName}ReadRepository>();
    services.AddScoped<I{EntityName}WriteRepository, {EntityName}WriteRepository>();

    return services;
}
```

**Example:**
```csharp
public static IServiceCollection AddRepositories(this IServiceCollection services)
{
    services.AddScoped<IUserReadRepository, UserReadRepository>();
    services.AddScoped<IUserWriteRepository, UserWriteRepository>();

    return services;
}
```

---

## Common Utilities Reference

### DbConstants (TicketingSystem.DataAccess)

**Location:** `src/TicketingSystem.DataAccess/DbConstants.cs`

**Available Constants:**
```csharp
public static class DbConstants
{
    public const string ConnectionStringName = "TicketingDb";
    public const int MaxEmailLength = 255;
    public const int ShortTextMaxLength = 255;
}
```

**Usage:** Add new constants as needed for your entity property max lengths.

### MappingExtensions (TicketingSystem.Common)

**Location:** `src/TicketingSystem.Common/Extensions/MappingExtensions.cs`

**Available Method:**
```csharp
public static TResult? MapIfNotNull<TSource, TResult>(this TSource? source, Func<TSource, TResult> mapFunc)
    where TSource : class
    where TResult : class
```

**Usage:** Null-safe mapping in repositories
```csharp
var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
```

### Automatic Audit Tracking

The `TicketingDbContext` automatically sets audit properties:
- **CreatedAt:** Set to `DateTimeOffset.UtcNow` when entity state is `Added`
- **LastModifiedAt:** Set to `DateTimeOffset.UtcNow` when entity state is `Modified`

No manual intervention required in repositories.

---

## Checklist for New Entity

Use this checklist when implementing a new entity:

- [ ] **Step 1:** Domain model created in `TicketingSystem.Domain/Models/{EntityName}.cs`
- [ ] **Step 2:** Read repository interface created in `TicketingSystem.Domain/Interfaces/Repositories/I{EntityName}ReadRepository.cs`
- [ ] **Step 2:** Write repository interface created in `TicketingSystem.Domain/Interfaces/Repositories/I{EntityName}WriteRepository.cs`
- [ ] **Step 3:** Entity created in `TicketingSystem.DataAccess/Entities/{EntityName}Entity.cs`
- [ ] **Step 4:** Entity configuration created in `TicketingSystem.DataAccess/EntityConfigurations/{EntityName}EntityConfiguration.cs`
- [ ] **Step 5:** DbSet added to `TicketingDbContext.cs`
- [ ] **Step 6:** Mapper created in `TicketingSystem.DataAccess/Mappers/{EntityName}Mapper.cs`
- [ ] **Step 7:** Read repository implemented in `TicketingSystem.DataAccess/Repositories/{EntityNamePlural}/{EntityName}ReadRepository.cs`
- [ ] **Step 8:** Write repository implemented in `TicketingSystem.DataAccess/Repositories/{EntityNamePlural}/{EntityName}WriteRepository.cs`
- [ ] **Step 9:** Repositories registered in `ServiceCollectionExtensions.AddRepositories()`
- [ ] **Database migration created** (using EF Core migrations)

---

## Example: Complete Entity Implementation (User)

This section shows the complete implementation of the User entity as a reference.

### Domain Layer (TicketingSystem.Domain)

**DomainModelBase.cs** (Base class for all domain models)
```csharp
namespace TicketingSystem.Domain.Models;

public abstract record DomainModelBase
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
```

**UserType.cs** (if enum is needed)
```csharp
namespace TicketingSystem.Domain;

public enum UserType
{
    Customer = 1,
    EventManager = 2
}
```

**Models/User.cs**
```csharp
namespace TicketingSystem.Domain.Models;

public sealed record User : DomainModelBase
{
    public UserType Type { get; set; }
    public required string Email { get; set; }
}
```

**Interfaces/Repositories/IUserReadRepository.cs**
```csharp
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IUserReadRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
```

**Interfaces/Repositories/IUserWriteRepository.cs**
```csharp
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IUserWriteRepository
{
    Task<Guid> AddAsync(User userModel, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(User userModel, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken);
}
```

### Data Access Layer (TicketingSystem.DataAccess)

**Entities/UserEntity.cs**
```csharp
using TicketingSystem.DataAccess.Entities.Abstractions;
using TicketingSystem.Domain;

namespace TicketingSystem.DataAccess.Entities;

internal sealed class UserEntity : AuditableEntityBase
{
    public UserType Type { get; set; }
    public required string Email { get; set; }
}
```

**EntityConfigurations/UserEntityConfiguration.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(DbConstants.MaxEmailLength);

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
```

**Mappers/UserMapper.cs**
```csharp
using Riok.Mapperly.Abstractions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Mappers;

[Mapper]
internal static partial class UserMapper
{
    [MapperIgnoreSource(nameof(UserEntity.IsDeleted))]
    public static partial User FromEntity(UserEntity userEntity);

    [MapperIgnoreTarget(nameof(UserEntity.IsDeleted))]
    public static partial UserEntity ToEntity(User userModel);
}
```

**Repositories/Users/UserReadRepository.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Users;

internal sealed class UserReadRepository : IUserReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<UserEntity> ActiveUsers 
        => _dbContext.Users.Where(x => !x.IsDeleted);

    public UserReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEntity = await ActiveUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
        return userModel;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEntity = await ActiveUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        var userModel = userEntity.MapIfNotNull(UserMapper.FromEntity);
        return userModel;
    }
}
```

**Repositories/Users/UserWriteRepository.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Users;

internal sealed class UserWriteRepository : IUserWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public UserWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(User userModel, CancellationToken cancellationToken)
    {
        var userEntity = UserMapper.ToEntity(userModel);
        userEntity.Id = Guid.NewGuid();
        await _dbContext.Users.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return userEntity.Id;
    }

    public async Task<bool> UpdateAsync(User userModel, CancellationToken cancellationToken)
    {
        var userEntity = await GetByIdAsync(userModel.Id, cancellationToken);
        if (userEntity == null)
            return false;

        userEntity.Type = userModel.Type;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = await GetByIdAsync(userId, cancellationToken);
        if (userEntity == null)
            return false;

        userEntity.IsDeleted = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}
```

---

## Additional Notes

### Soft Delete Pattern
- All entities inherit `IsDeleted` property from `DbEntityBase`
- Repositories filter out deleted records using `.Where(x => !x.IsDeleted)`
- Delete operations set `IsDeleted = true` instead of removing records
- Unique indexes should include filter: `.HasFilter("[IsDeleted] = 0")`

### Naming Conventions
- **Domain Models:** Singular, PascalCase (e.g., `User`, `Customer`)
- **Entities:** Singular with `Entity` suffix (e.g., `UserEntity`, `CustomerEntity`)
- **Repositories:** Singular with repository type (e.g., `UserReadRepository`, `UserWriteRepository`)
- **Repository Folders:** Plural (e.g., `Users/`, `Customers/`)
- **Database Tables:** Plural (e.g., `Users`, `Customers`)
- **Mappers:** Singular with `Mapper` suffix (e.g., `UserMapper`, `CustomerMapper`)

### Project Dependencies
- **TicketingSystem.Domain** - No dependencies on other projects (pure domain)
- **TicketingSystem.DataAccess** - Depends on Domain
- **TicketingSystem.Common** - Shared utilities (no domain dependencies)

---

## Summary

Following this pattern ensures:
- ✅ **Separation of Concerns** - Domain models are isolated from persistence details
- ✅ **Consistency** - All entities follow the same structure and conventions
- ✅ **Maintainability** - Clear organization and predictable file locations
- ✅ **Testability** - Repository pattern enables easy mocking and testing
- ✅ **Soft Delete Support** - Built-in soft delete capability for all entities
- ✅ **Audit Tracking** - Automatic CreatedAt/LastModifiedAt tracking

For questions or clarifications about this pattern, refer to the User entity implementation as the reference example.
