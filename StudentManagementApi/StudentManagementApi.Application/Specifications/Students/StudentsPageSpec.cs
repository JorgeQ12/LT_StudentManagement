using Ardalis.Specification;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentsPageSpec : Specification<Student>
{
    public StudentsPageSpec(int pageNumber, int pageSize) =>
        Query.Include(student => student.Account)
            .OrderBy(student => student.LastName)
            .ThenBy(student => student.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
}
