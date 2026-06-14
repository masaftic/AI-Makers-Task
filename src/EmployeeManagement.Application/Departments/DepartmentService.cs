using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.Departments.Dtos;
using EmployeeManagement.Domain.DepartmentRoot;
using EmployeeManagement.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Departments;

public interface IDepartmentService
{
    Task<Result<Guid>> CreateAsync(SaveDepartmentDto input, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, SaveDepartmentDto input, CancellationToken cancellationToken = default);
}

public sealed class DepartmentService(IAppDbContext db) : IDepartmentService
{
    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await db.Departments
            .AsNoTracking()
            .OrderBy(department => department.Name)
            .ToDto()
            .ToListAsync(cancellationToken);
    }

    public async Task<DepartmentDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await db.Departments
            .AsNoTracking()
            .Where(department => department.Id == id)
            .ToDto()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Result<Guid>> CreateAsync(
        SaveDepartmentDto input,
        CancellationToken cancellationToken = default)
    {
        var name = input.Name.Trim();
        if (await db.Departments.NameExistsAsync(
                name,
                cancellationToken: cancellationToken))
        {
            return AppError.Conflict(
                "Department.Name.Exists",
                "A department with this name already exists.");
        }

        var department = Department.Create(Guid.NewGuid(), name);

        await db.Departments.AddAsync(department, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return department.Id;
    }

    public async Task<Result> UpdateAsync(
        Guid id,
        SaveDepartmentDto input,
        CancellationToken cancellationToken = default)
    {
        var department = await db.Departments.FindByIdAsync(id, cancellationToken);
        if (department is null)
        {
            return AppError.NotFound("Department.NotFound", "Department not found.");
        }

        var name = input.Name.Trim();
        if (await db.Departments.NameExistsAsync(name, id, cancellationToken))
        {
            return AppError.Conflict(
                "Department.Name.Exists",
                "A department with this name already exists.");
        }

        department.Update(name);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var department = await db.Departments.FindByIdAsync(id, cancellationToken);
        if (department is null)
        {
            return AppError.NotFound("Department.NotFound", "Department not found.");
        }

        db.Departments.Remove(department);
        await db.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}
