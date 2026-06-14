using EmployeeManagement.Domain.DepartmentRoot;
using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Common;

public interface IAppDbContext
{
    DbSet<Employee> Employees { get; }
    DbSet<Department> Departments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
