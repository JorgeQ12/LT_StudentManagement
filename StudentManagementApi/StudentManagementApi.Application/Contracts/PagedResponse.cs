namespace StudentManagementApi.Application.Contracts;

public sealed record PagedResponse<T>(IReadOnlyCollection<T> Items, int PageNumber, int PageSize, int TotalCount);
