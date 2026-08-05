namespace StudentManagementApi.Domain.Abstractions;

public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id), IAggregateRoot
    where TId : notnull
{
    public byte[] RowVersion { get; private set; } = [];
}
