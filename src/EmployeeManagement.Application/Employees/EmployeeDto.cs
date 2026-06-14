namespace EmployeeManagement.Application.Employees;

public sealed record EmployeeDto(
    Guid Id,
    string FullName,
    string Email,
    string MobileNumber,
    Guid DepartmentId,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
