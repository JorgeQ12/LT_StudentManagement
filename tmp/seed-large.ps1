param(
    [Parameter(Mandatory)] [string]$BaseUrl,
    [Parameter(Mandatory)] [string]$AdminEmail,
    [Parameter(Mandatory)] [string]$AdminPassword,
    [int]$StudentCount = 48,
    [int]$EnrollmentCount = 8
)

$ErrorActionPreference = "Stop"
$studentPassword = "Student12345!"

function New-ApiSession {
    $script:session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
    $script:csrf = $null
}

function Get-CsrfToken {
    (Invoke-RestMethod `
        -Uri "$BaseUrl/api/Authentication/GenerateAntiforgeryToken" `
        -WebSession $script:session).token
}

function Invoke-WriteRequest {
    param(
        [Parameter(Mandatory)] [string]$Path,
        [Parameter(Mandatory)] [object]$Body,
        [string]$Method = "Post"
    )

    Invoke-RestMethod `
        -Uri "$BaseUrl$Path" `
        -Method $Method `
        -WebSession $script:session `
        -Headers @{ "X-CSRF-TOKEN" = $script:csrf } `
        -ContentType "application/json" `
        -Body ($Body | ConvertTo-Json -Depth 10)
}

function Connect-User {
    param([string]$Email, [string]$Password)

    New-ApiSession
    $script:csrf = Get-CsrfToken

    Invoke-WriteRequest `
        -Path "/api/Authentication/Login" `
        -Body @{ email = $Email; password = $Password } | Out-Null

    # Debe renovarse después de cambiar la identidad.
    $script:csrf = Get-CsrfToken
}

Write-Host "Iniciando sesión como administrador..."
Connect-User -Email $AdminEmail -Password $AdminPassword

$programDefinitions = @(
    @{ Code = "SWE"; Name = "Ingeniería de Software" }
    @{ Code = "SYS"; Name = "Ingeniería de Sistemas" }
    @{ Code = "ADS"; Name = "Análisis y Desarrollo de Software" }
    @{ Code = "DAT"; Name = "Ciencia de Datos" }
    @{ Code = "NET"; Name = "Redes y Telecomunicaciones" }
    @{ Code = "SEC"; Name = "Ciberseguridad" }
)

$professorDefinitions = @(
    @{ FirstName = "Ana"; LastName = "Martínez" }
    @{ FirstName = "Carlos"; LastName = "Ramírez" }
    @{ FirstName = "Laura"; LastName = "Gómez" }
    @{ FirstName = "Miguel"; LastName = "Torres" }
    @{ FirstName = "Sofía"; LastName = "Herrera" }
    @{ FirstName = "Daniel"; LastName = "Castro" }
    @{ FirstName = "Valentina"; LastName = "Rojas" }
    @{ FirstName = "Andrés"; LastName = "Moreno" }
    @{ FirstName = "Camila"; LastName = "Vargas" }
    @{ FirstName = "Sebastián"; LastName = "Ortiz" }
    @{ FirstName = "Natalia"; LastName = "Mendoza" }
    @{ FirstName = "Alejandro"; LastName = "Navarro" }
)

$courseNames = @(
    "Fundamentos de Programación",
    "Bases de Datos",
    "Arquitectura de Software",
    "Desarrollo Web",
    "Calidad de Software",
    "Computación en la Nube"
)

Write-Host "Creando profesores..."
$professors = foreach ($definition in $professorDefinitions) {
    Invoke-WriteRequest `
        -Path "/api/AdministrationProfessors/CreateProfessor" `
        -Body @{
            firstName = $definition.FirstName
            lastName  = $definition.LastName
        }
}

Write-Host "Creando programas, cursos y asignaciones..."
$programs = @()
$coursesByProgram = @{}

for ($programIndex = 0; $programIndex -lt $programDefinitions.Count; $programIndex++) {
    $definition = $programDefinitions[$programIndex]

    $program = Invoke-WriteRequest `
        -Path "/api/AdministrationAcademicPrograms/CreateAcademicProgram" `
        -Body @{
            code        = $definition.Code
            name        = $definition.Name
            description = "Programa de $($definition.Name) para demostración académica."
        }

    $programs += $program
    $programCourses = @()

    for ($courseIndex = 0; $courseIndex -lt 6; $courseIndex++) {
        $course = Invoke-WriteRequest `
            -Path "/api/AdministrationCourses/CreateCourse" `
            -Body @{
                academicProgramId = $program.id
                code              = "$($definition.Code)$($courseIndex + 101)"
                name              = $courseNames[$courseIndex]
            }

        $programCourses += $course

        # Tres profesores distintos por programa, dos cursos para cada uno.
        $professorOffset = ($programIndex * 3) % $professors.Count
        $professorIndex = ($professorOffset + ($courseIndex % 3)) % $professors.Count

        Invoke-WriteRequest `
            -Path "/api/AdministrationCourses/AssignProfessor" `
            -Method "Put" `
            -Body @{
                courseId    = $course.id
                professorId = $professors[$professorIndex].id
            } | Out-Null
    }

    $coursesByProgram[$definition.Code] = $programCourses
}

Write-Host "Creando $StudentCount estudiantes..."
$students = @()

for ($index = 1; $index -le $StudentCount; $index++) {
    $number = $index.ToString("000")
    $programIndex = ($index - 1) % $programs.Count
    $birthDate = (Get-Date "1998-01-01").AddDays($index * 43).ToString("yyyy-MM-dd")

    $student = @{
        Email        = "student$number@studentmanagement.local"
        Password     = $studentPassword
        ProgramIndex = $programIndex
    }

    Invoke-WriteRequest `
        -Path "/api/AdministrationStudents/CreateStudent" `
        -Body @{
            firstName      = "Estudiante$number"
            lastName       = "Demostración"
            documentNumber = "DOC100$number"
            dateOfBirth    = $birthDate
            phoneNumber    = "3001000$number"
            email          = $student.Email
            password       = $student.Password
        } | Out-Null

    $students += $student
}

$enrollmentsToCreate = [Math]::Min($EnrollmentCount, [Math]::Min(8, $students.Count))

Write-Host "Creando $enrollmentsToCreate matrículas..."
for ($index = 0; $index -lt $enrollmentsToCreate; $index++) {
    $student = $students[$index]
    $program = $programs[$student.ProgramIndex]
    $programCode = $programDefinitions[$student.ProgramIndex].Code
    $courses = $coursesByProgram[$programCode]

    Connect-User -Email $student.Email -Password $student.Password

    Invoke-WriteRequest `
        -Path "/api/Enrollments/CreateCurrentStudentEnrollment" `
        -Body @{
            academicProgramId = $program.id
            courseIds = @(
                $courses[0].id
                $courses[1].id
                $courses[2].id
            )
        } | Out-Null
}

Write-Host ""
Write-Host "Población completada:" -ForegroundColor Green
Write-Host "- $($programs.Count) programas"
Write-Host "- $($professors.Count) profesores"
Write-Host "- $($programs.Count * 6) cursos"
Write-Host "- $($students.Count) estudiantes"
Write-Host "- $enrollmentsToCreate matrículas"
Write-Host ""
Write-Host "Contraseña común de estudiantes: $studentPassword"