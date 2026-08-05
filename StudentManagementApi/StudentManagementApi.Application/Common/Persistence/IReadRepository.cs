using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.Common.Persistence;

public interface IReadRepository<TAggregate> : IReadRepositoryBase<TAggregate>
    where TAggregate : class, IAggregateRoot;
