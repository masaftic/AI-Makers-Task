using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.Departments;
using EmployeeManagement.Application.Employees.Dtos;
using EmployeeManagement.Domain.EmployeeRoot;
using EmployeeManagement.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Employees;

public interface IEmployeeService
{
    Task<Result<int>> CreateAsync(SaveEmployeeDto input, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EmployeeDto>> GetAllAsync(
        string? name = null,
        int? departmentId = null,
        CancellationToken cancellationToken = default);
    Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, SaveEmployeeDto input, CancellationToken cancellationToken = default);
}


public sealed class EmployeeService(IAppDbContext dbContext) : IEmployeeService
{
    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(
        string? name = null,
        int? departmentId = null,
        CancellationToken cancellationToken = default)
    {
        var employees = await dbContext.Employees
            .AsNoTracking()
            .SearchByName(name)
            .FilterByDepartment(departmentId)
            .OrderBy(employee => employee.FullName)
            .ToProjection()
            .ToListAsync(cancellationToken);

        return employees.Select(employee => employee.ToDto()).ToList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var employee = await dbContext.Employees
            .AsNoTracking()
            .Where(employee => employee.Id == id)
            .ToProjection()
            .SingleOrDefaultAsync(cancellationToken);

        return employee?.ToDto();
    }

    public async Task<Result<int>> CreateAsync(
        SaveEmployeeDto input,
        CancellationToken cancellationToken = default)
    {
        var email = EmailAddress.Create(input.Email);
        var mobileNumber = MobileNumber.Create(input.MobileNumber);
        var conflicts = await FindConflictsAsync(email, mobileNumber, null, cancellationToken);

        if (input.DepartmentId.HasValue &&
            !await dbContext.Departments.ExistsAsync(input.DepartmentId.Value, cancellationToken))
        {
            conflicts.Add(AppError.Validation(
                "Employee.Department.NotFound",
                "Select an existing department."));
        }

        if (conflicts.Count > 0)
        {
            return conflicts;
        }

        var employee = Employee.Create(
            0, input.FullName.Trim(), email, mobileNumber,
            input.DepartmentId, input.JobTitle.Trim(), input.HireDate, input.IsActive);

        await dbContext.Employees.AddAsync(employee, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }

    public async Task<Result> UpdateAsync(
        int id,
        SaveEmployeeDto input,
        CancellationToken cancellationToken = default)
    {
        var employee = await dbContext.Employees.FindAsync([id], cancellationToken);
        if (employee is null)
        {
            return AppError.NotFound("Employee.NotFound", "Employee not found.");
        }

        var email = EmailAddress.Create(input.Email);
        var mobileNumber = MobileNumber.Create(input.MobileNumber);
        var conflicts = await FindConflictsAsync(email, mobileNumber, id, cancellationToken);

        if (input.DepartmentId.HasValue &&
            !await dbContext.Departments.ExistsAsync(input.DepartmentId.Value, cancellationToken))
        {
            conflicts.Add(AppError.Validation(
                "Employee.Department.NotFound",
                "Select an existing department."));
        }

        if (conflicts.Count > 0)
        {
            return conflicts;
        }

        employee.Update(
            input.FullName.Trim(),
            email,
            mobileNumber,
            input.DepartmentId,
            input.JobTitle.Trim(),
            input.HireDate,
            input.IsActive);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var employee = await dbContext.Employees.FindAsync([id], cancellationToken);
        if (employee is null)
        {
            return AppError.NotFound("Employee.NotFound", "Employee not found.");
        }

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private async Task<List<AppError>> FindConflictsAsync(
        EmailAddress email,
        MobileNumber mobileNumber,
        int? excludingEmployeeId,
        CancellationToken cancellationToken)
    {
        List<AppError> errors = [];

        if (await dbContext.Employees.EmailExistsAsync(
                email, excludingEmployeeId, cancellationToken))
        {
            errors.Add(AppError.Conflict("Employee.Email.Exists", "Email is already in use."));
        }

        if (await dbContext.Employees.MobileNumberExistsAsync(
                mobileNumber, excludingEmployeeId, cancellationToken))
        {
            errors.Add(AppError.Conflict(
                "Employee.MobileNumber.Exists",
                "Mobile number is already in use."));
        }

        return errors;
    }
}
