using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.Common.Persistence;

public interface IWriteRepository<TAggregate> : IRepositoryBase<TAggregate>
    where TAggregate : class, IAggregateRoot;
