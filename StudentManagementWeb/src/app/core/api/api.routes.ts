const apiAction = (controller: string, action: string) => `${controller}/${action}`;

export const API_ROUTES = {
  authentication: {
    antiforgery: apiAction('Authentication', 'GenerateAntiforgeryToken'),
    register: apiAction('Authentication', 'RegisterStudent'),
    login: apiAction('Authentication', 'Login'),
    logout: apiAction('Authentication', 'Logout'),
    authenticatedUser: apiAction('Authentication', 'GetAuthenticatedUser'),
  },
  studentProfiles: {
    current: apiAction('StudentProfiles', 'GetCurrentStudentProfile'),
    update: apiAction('StudentProfiles', 'UpdateCurrentStudentProfile'),
    deactivate: apiAction('StudentProfiles', 'DeactivateCurrentStudentAccount'),
  },
  enrollments: {
    create: apiAction('Enrollments', 'CreateCurrentStudentEnrollment'),
    current: apiAction('Enrollments', 'GetCurrentStudentEnrollment'),
    replace: apiAction('Enrollments', 'ReplaceCurrentStudentSelectedCourses'),
    cancel: apiAction('Enrollments', 'CancelCurrentStudentEnrollment'),
    classmates: apiAction('Enrollments', 'GetCurrentStudentClassmatesByCourse'),
  },
  academicPrograms: {
    active: apiAction('AcademicPrograms', 'GetActiveAcademicPrograms'),
  },
  courses: {
    activeByProgram: apiAction('Courses', 'GetActiveCoursesByAcademicProgram'),
  },
  administration: {
    students: {
      all: apiAction('AdministrationStudents', 'GetAllStudents'),
      byId: apiAction('AdministrationStudents', 'GetStudentById'),
      create: apiAction('AdministrationStudents', 'CreateStudent'),
      update: apiAction('AdministrationStudents', 'UpdateStudent'),
      activate: apiAction('AdministrationStudents', 'ActivateStudent'),
      deactivate: apiAction('AdministrationStudents', 'DeactivateStudent'),
    },
    academicPrograms: {
      all: apiAction('AdministrationAcademicPrograms', 'GetAllAcademicPrograms'),
      byId: apiAction('AdministrationAcademicPrograms', 'GetAcademicProgramById'),
      create: apiAction('AdministrationAcademicPrograms', 'CreateAcademicProgram'),
      update: apiAction('AdministrationAcademicPrograms', 'UpdateAcademicProgram'),
      activate: apiAction('AdministrationAcademicPrograms', 'ActivateAcademicProgram'),
      deactivate: apiAction('AdministrationAcademicPrograms', 'DeactivateAcademicProgram'),
    },
    courses: {
      all: apiAction('AdministrationCourses', 'GetAllCourses'),
      byId: apiAction('AdministrationCourses', 'GetCourseById'),
      create: apiAction('AdministrationCourses', 'CreateCourse'),
      update: apiAction('AdministrationCourses', 'UpdateCourse'),
      activate: apiAction('AdministrationCourses', 'ActivateCourse'),
      deactivate: apiAction('AdministrationCourses', 'DeactivateCourse'),
      assignProfessor: apiAction('AdministrationCourses', 'AssignProfessor'),
    },
    professors: {
      all: apiAction('AdministrationProfessors', 'GetAllProfessors'),
      byId: apiAction('AdministrationProfessors', 'GetProfessorById'),
      create: apiAction('AdministrationProfessors', 'CreateProfessor'),
      update: apiAction('AdministrationProfessors', 'UpdateProfessor'),
      activate: apiAction('AdministrationProfessors', 'ActivateProfessor'),
      deactivate: apiAction('AdministrationProfessors', 'DeactivateProfessor'),
    },
  },
} as const;
