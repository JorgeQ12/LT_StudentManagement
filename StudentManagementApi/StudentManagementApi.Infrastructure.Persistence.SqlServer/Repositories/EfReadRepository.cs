using Ardalis.Specification.EntityFrameworkCore;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Repositories;

internal sealed class EfReadRepository<TAggregate>(StudentManagementReadDbContext dbContext)
    : RepositoryBase<TAggregate>(dbContext), IReadRepository<TAggregate> where TAggregate : class, IAggregateRoot;
