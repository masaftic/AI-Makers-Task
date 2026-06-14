using EmployeeManagement.Application.Departments.Dtos;
using EmployeeManagement.Domain.DepartmentRoot;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Departments;

public static class DepartmentQueryExtensions
{
    extension(DbSet<Department> departments)
    {
        public ValueTask<Department?> FindByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => departments.FindAsync([id], cancellationToken);
    }

    extension(IQueryable<Department> queryable)
    {
        public IQueryable<DepartmentDto> ToDto()
            => queryable.Select(department => new DepartmentDto(
                department.Id,
                department.Name));

        public Task<bool> NameExistsAsync(
            string name,
            Guid? excludingDepartmentId = null,
            CancellationToken cancellationToken = default)
            => queryable.AnyAsync(
                department =>
                    department.Name == name &&
                    (!excludingDepartmentId.HasValue ||
                     department.Id != excludingDepartmentId.Value),
                cancellationToken);

        public Task<bool> ExistsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => queryable.AnyAsync(
                department => department.Id == id,
                cancellationToken);
    }
}
