namespace EmployeeManagement.Application.Employees.Dtos;

public sealed record SaveEmployeeDto(
    string FullName,
    string Email,
    string MobileNumber,
    Guid? DepartmentId,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
