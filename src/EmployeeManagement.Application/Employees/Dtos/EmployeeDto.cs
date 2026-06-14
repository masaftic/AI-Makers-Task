namespace EmployeeManagement.Application.Employees.Dtos;

public sealed record EmployeeDto(
    int Id,
    string FullName,
    string Email,
    string MobileNumber,
    int? DepartmentId,
    string? DepartmentName,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
