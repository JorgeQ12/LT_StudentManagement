import { readFileSync } from 'node:fs';

const document = JSON.parse(readFileSync(new URL('../docs/openapi.json', import.meta.url), 'utf8'));

const expectedOperations = [
  ['GET', '/api/Authentication/GenerateAntiforgeryToken'],
  ['POST', '/api/Authentication/RegisterStudent'],
  ['POST', '/api/Authentication/Login'],
  ['POST', '/api/Authentication/Logout'],
  ['GET', '/api/Authentication/GetAuthenticatedUser'],
  ['GET', '/api/AcademicPrograms/GetActiveAcademicPrograms'],
  ['GET', '/api/Courses/GetActiveCoursesByAcademicProgram'],
  ['POST', '/api/Enrollments/CreateCurrentStudentEnrollment'],
  ['GET', '/api/Enrollments/GetCurrentStudentEnrollment'],
  ['PUT', '/api/Enrollments/ReplaceCurrentStudentSelectedCourses'],
  ['DELETE', '/api/Enrollments/CancelCurrentStudentEnrollment'],
  ['GET', '/api/Enrollments/GetCurrentStudentClassmatesByCourse'],
  ['GET', '/api/StudentProfiles/GetCurrentStudentProfile'],
  ['PUT', '/api/StudentProfiles/UpdateCurrentStudentProfile'],
  ['DELETE', '/api/StudentProfiles/DeactivateCurrentStudentAccount'],
  ['GET', '/api/AdministrationAcademicPrograms/GetAllAcademicPrograms'],
  ['GET', '/api/AdministrationAcademicPrograms/GetAcademicProgramById'],
  ['POST', '/api/AdministrationAcademicPrograms/CreateAcademicProgram'],
  ['PUT', '/api/AdministrationAcademicPrograms/UpdateAcademicProgram'],
  ['PUT', '/api/AdministrationAcademicPrograms/ActivateAcademicProgram'],
  ['DELETE', '/api/AdministrationAcademicPrograms/DeactivateAcademicProgram'],
  ['GET', '/api/AdministrationCourses/GetAllCourses'],
  ['GET', '/api/AdministrationCourses/GetCourseById'],
  ['POST', '/api/AdministrationCourses/CreateCourse'],
  ['PUT', '/api/AdministrationCourses/UpdateCourse'],
  ['PUT', '/api/AdministrationCourses/ActivateCourse'],
  ['DELETE', '/api/AdministrationCourses/DeactivateCourse'],
  ['PUT', '/api/AdministrationCourses/AssignProfessor'],
  ['GET', '/api/AdministrationProfessors/GetAllProfessors'],
  ['GET', '/api/AdministrationProfessors/GetProfessorById'],
  ['POST', '/api/AdministrationProfessors/CreateProfessor'],
  ['PUT', '/api/AdministrationProfessors/UpdateProfessor'],
  ['PUT', '/api/AdministrationProfessors/ActivateProfessor'],
  ['DELETE', '/api/AdministrationProfessors/DeactivateProfessor'],
  ['GET', '/api/AdministrationStudents/GetAllStudents'],
  ['GET', '/api/AdministrationStudents/GetStudentById'],
  ['POST', '/api/AdministrationStudents/CreateStudent'],
  ['PUT', '/api/AdministrationStudents/UpdateStudent'],
  ['PUT', '/api/AdministrationStudents/ActivateStudent'],
  ['DELETE', '/api/AdministrationStudents/DeactivateStudent'],
];

const requiredSchemas = [
  'AuthenticatedUserResponse',
  'EnrollmentResponse',
  'PagedResponseOfAcademicProgramResponse',
  'PagedResponseOfCourseResponse',
  'PagedResponseOfProfessorResponse',
  'PagedResponseOfStudentResponse',
  'ProblemDetails',
  'HttpValidationProblemDetails',
];

const missingOperations = expectedOperations.filter(
  ([method, path]) => !document.paths?.[path]?.[method.toLowerCase()],
);
const missingSchemas = requiredSchemas.filter((name) => !document.components?.schemas?.[name]);

if (!String(document.openapi).startsWith('3.1')) {
  throw new Error(`Expected OpenAPI 3.1, received ${document.openapi ?? 'unknown'}.`);
}
if (missingOperations.length) {
  throw new Error(
    `Missing operations:\n${missingOperations.map(([method, path]) => `${method} ${path}`).join('\n')}`,
  );
}
if (missingSchemas.length) {
  throw new Error(`Missing response schemas: ${missingSchemas.join(', ')}`);
}

console.log(
  `OpenAPI contract verified: ${expectedOperations.length} operations and ${Object.keys(document.components.schemas).length} schemas.`,
);
