using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Specifications;

internal sealed class StudentByIdSpec : SingleResultSpecification<Student>
{
    public StudentByIdSpec(StudentId id) => Query.Where(student => student.Id == id).Include(student => student.Account);
}
