using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.StudentProfiles.GetCurrentStudentProfile;

public sealed record GetCurrentStudentProfileQuery : IQuery<StudentResponse>;
