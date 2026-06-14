using EmployeeManagement.Domain.EmployeeRoot;

namespace EmployeeManagement.Application.Employees;

public sealed record EmployeeProjection(
    int Id,
    string FullName,
    EmailAddress Email,
    MobileNumber MobileNumber,
    int? DepartmentId,
    string? DepartmentName,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
