using EmployeeManagement.Application.Common;
using EmployeeManagement.Domain.DepartmentRoot;
using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence;

public sealed class EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options)
    : DbContext(options), IAppDbContext
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
}
