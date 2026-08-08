using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Students;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentsPageSpec : Specification<Student, StudentResponse>
{
    public StudentsPageSpec(int pageNumber, int pageSize, string? search, AccountStatus? status)
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
            .OrderBy(student => student.LastName)
            .ThenBy(student => student.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Select(student => new StudentResponse(
                student.Id.Value,
                student.FirstName,
                student.LastName,
                student.DocumentNumber.Value,
                student.DateOfBirth,
                student.PhoneNumber.Value,
                student.Account.Email.Value,
                student.Status));
    }
}
