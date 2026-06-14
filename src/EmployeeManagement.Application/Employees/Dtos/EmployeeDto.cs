namespace EmployeeManagement.Application.Employees.Dtos;

public sealed record EmployeeDto(
    Guid Id,
    string FullName,
    string Email,
    string MobileNumber,
    Guid? DepartmentId,
    string? DepartmentName,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
