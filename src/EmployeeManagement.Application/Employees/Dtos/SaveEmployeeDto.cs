namespace EmployeeManagement.Application.Employees.Dtos;

public sealed record SaveEmployeeDto(
    string FullName,
    string Email,
    string MobileNumber,
    int? DepartmentId,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
