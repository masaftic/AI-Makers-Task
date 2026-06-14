namespace EmployeeManagement.Application.Employees;

public sealed record SaveEmployeeDto(
    string FullName,
    string Email,
    string MobileNumber,
    Guid DepartmentId,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
