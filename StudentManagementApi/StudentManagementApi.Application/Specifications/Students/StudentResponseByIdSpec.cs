using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentResponseByIdSpec : SingleResultSpecification<Student, StudentResponse>
{
    public StudentResponseByIdSpec(StudentId studentId) =>
        Query.Where(student => student.Id == studentId)
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
