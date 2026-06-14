using EmployeeManagement.Application.Employees.Dtos;
using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Employees;

public static class EmployeeQueryExtensions
{
    extension(IQueryable<Employee> queryable)
    {
        public IQueryable<EmployeeProjection> ToProjection()
            => queryable.Select(employee => new EmployeeProjection(
                employee.Id,
                employee.FullName,
                employee.Email,
                employee.MobileNumber,
                employee.DepartmentId,
                employee.Department == null ? null : employee.Department.Name,
                employee.JobTitle,
                employee.HireDate,
                employee.IsActive));

        public Task<bool> EmailExistsAsync(
            EmailAddress email,
            Guid? excludingEmployeeId = null,
            CancellationToken cancellationToken = default)
            => queryable.AnyAsync(
                employee =>
                    employee.Email == email &&
                    (!excludingEmployeeId.HasValue ||
                     employee.Id != excludingEmployeeId.Value),
                cancellationToken);

        public Task<bool> MobileNumberExistsAsync(
            MobileNumber mobileNumber,
            Guid? excludingEmployeeId = null,
            CancellationToken cancellationToken = default)
            => queryable.AnyAsync(
                employee =>
                    employee.MobileNumber == mobileNumber &&
                    (!excludingEmployeeId.HasValue ||
                     employee.Id != excludingEmployeeId.Value),
                cancellationToken);
    }

    extension(EmployeeProjection employee)
    {
        public EmployeeDto ToDto()
            => new(
                employee.Id,
                employee.FullName,
                employee.Email.Value,
                employee.MobileNumber.Value,
                employee.DepartmentId,
                employee.DepartmentName,
                employee.JobTitle,
                employee.HireDate,
                employee.IsActive);
    }
}
