using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentsPageSpec : Specification<Student>
{
    public StudentsPageSpec(int pageNumber, int pageSize, string? search, AccountStatus? status)
    {
        Query.Include(student => student.Account)
            .Where(student =>
                (search == null
                    || student.FirstName.Contains(search)
                    || student.LastName.Contains(search)
                    || student.DocumentNumber.Value.Contains(search)
                    || student.Account.Email.Value.Contains(search))
                && (status == null || student.Status == status.Value))
            .OrderBy(student => student.LastName)
            .ThenBy(student => student.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
    }
}
