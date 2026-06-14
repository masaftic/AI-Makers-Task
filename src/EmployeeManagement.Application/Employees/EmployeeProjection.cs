using EmployeeManagement.Domain.EmployeeRoot;

namespace EmployeeManagement.Application.Employees;

public sealed record EmployeeProjection(
    Guid Id,
    string FullName,
    EmailAddress Email,
    MobileNumber MobileNumber,
    Guid DepartmentId,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive);
