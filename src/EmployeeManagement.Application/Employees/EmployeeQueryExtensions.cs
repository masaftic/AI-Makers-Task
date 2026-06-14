using EmployeeManagement.Application.Employees.Dtos;
using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Employees;

public static class EmployeeQueryExtensions
{
    extension(IQueryable<Employee> queryable)
    {
        public IQueryable<Employee> SearchByName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return queryable;
            }

            var term = name.Trim();
            return queryable.Where(employee => employee.FullName.Contains(term));
        }

        public IQueryable<Employee> FilterByDepartment(int? departmentId)
        {
            if (!departmentId.HasValue)
            {
                return queryable;
            }

            return queryable.Where(employee => employee.DepartmentId == departmentId.Value);
        }

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
            int? excludingEmployeeId = null,
            CancellationToken cancellationToken = default)
            => queryable.AnyAsync(
                employee =>
                    employee.Email == email &&
                    (!excludingEmployeeId.HasValue ||
                     employee.Id != excludingEmployeeId.Value),
                cancellationToken);

        public Task<bool> MobileNumberExistsAsync(
            MobileNumber mobileNumber,
            int? excludingEmployeeId = null,
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
