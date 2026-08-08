using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Students;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentsCountSpec : Specification<Student>
{
    public StudentsCountSpec(string? search, AccountStatus? status)
    {
        var normalizedSearch = SearchValueObject.Normalize(search);
        var documentNumber = SearchValueObject.TryCreate(normalizedSearch, DocumentNumber.Create);
        var email = SearchValueObject.TryCreate(normalizedSearch, Email.Create);

        Query.Where(student =>
                (normalizedSearch == null
                    || student.FirstName.Contains(normalizedSearch)
                    || student.LastName.Contains(normalizedSearch)
                    || (documentNumber != null && student.DocumentNumber == documentNumber)
                    || (email != null && student.Account.Email == email))
                && (status == null || student.Status == status.Value))
            .AsNoTracking();
    }
}
