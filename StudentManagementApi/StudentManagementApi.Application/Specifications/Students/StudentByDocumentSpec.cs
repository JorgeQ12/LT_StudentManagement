using Ardalis.Specification;
using StudentManagementApi.Domain.Students;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentByDocumentSpec : SingleResultSpecification<Student>
{
    public StudentByDocumentSpec(string documentNumber)
    {
        var normalizedDocumentNumber = DocumentNumber.Create(documentNumber);
        Query.Where(student => student.DocumentNumber == normalizedDocumentNumber);
    }
}
