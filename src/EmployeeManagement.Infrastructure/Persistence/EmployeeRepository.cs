using System.Linq.Expressions;
using EmployeeManagement.Application.Employees;
using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence;

public sealed class EmployeeRepository(EmployeeManagementDbContext dbContext)
    : IEmployeeRepository
{
    public async Task<IReadOnlyList<EmployeeProjection>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.FullName)
            .ToProjection()
            .ToListAsync(cancellationToken);
    }

    public Task<EmployeeProjection?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Employees
            .AsNoTracking()
            .Where(employee => employee.Id == id)
            .ToProjection()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<Employee?> GetForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Employees.SingleOrDefaultAsync(
            employee => employee.Id == id,
            cancellationToken);
    }

    public async Task AddAsync(
        Employee employee,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Employees.AddAsync(employee, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(
        EmailAddress email,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Employees.AnyAsync(
            employee =>
                employee.Email == email &&
                (!excludingEmployeeId.HasValue || employee.Id != excludingEmployeeId.Value),
            cancellationToken);
    }

    public Task<bool> MobileNumberExistsAsync(
        MobileNumber mobileNumber,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Employees.AnyAsync(
            employee =>
                employee.MobileNumber == mobileNumber &&
                (!excludingEmployeeId.HasValue || employee.Id != excludingEmployeeId.Value),
            cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}

public static class EmployeeRepositoryExtensions
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
                employee.JobTitle,
                employee.HireDate,
                employee.IsActive));
    }
}
