using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentsCountSpec : Specification<Student>
{
    public StudentsCountSpec(string? search, AccountStatus? status)
    {
        Query.Where(student =>
                (search == null
                    || student.FirstName.Contains(search)
                    || student.LastName.Contains(search)
                    || student.DocumentNumber.Value.Contains(search)
                    || student.Account.Email.Value.Contains(search))
                && (status == null || student.Status == status.Value))
            .AsNoTracking();
    }
}
