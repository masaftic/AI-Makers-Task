using EmployeeManagement.Domain.EmployeeRoot;
using EmployeeManagement.Domain.Shared;

namespace EmployeeManagement.Application.Employees;

public sealed class EmployeeService(IEmployeeRepository employeeRepository)
{
    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var employees = await employeeRepository.GetAllAsync(cancellationToken);

        return employees
            .Select(employee => new EmployeeDto(
                employee.Id, employee.FullName, employee.Email.Value, employee.MobileNumber.Value,
                employee.DepartmentId, employee.JobTitle, employee.HireDate, employee.IsActive))
            .ToList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetByIdAsync(id, cancellationToken);

        return employee is null
            ? null
            : new EmployeeDto(
                employee.Id, employee.FullName, employee.Email.Value, employee.MobileNumber.Value,
                employee.DepartmentId, employee.JobTitle, employee.HireDate, employee.IsActive);
    }

    public async Task<Result<Guid>> CreateAsync(
        SaveEmployeeDto input,
        CancellationToken cancellationToken = default)
    {
        var email = EmailAddress.Create(input.Email);
        var mobileNumber = MobileNumber.Create(input.MobileNumber);
        var conflicts = await FindConflictsAsync(email, mobileNumber, null, cancellationToken);

        if (conflicts.Count > 0)
        {
            return conflicts;
        }

        var employee = Employee.Create(
            Guid.NewGuid(), input.FullName.Trim(), email, mobileNumber,
            input.DepartmentId, input.JobTitle.Trim(), input.HireDate, input.IsActive);

        await employeeRepository.AddAsync(employee, cancellationToken);
        await employeeRepository.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }

    public async Task<Result> UpdateAsync(
        Guid id,
        SaveEmployeeDto input,
        CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetForUpdateAsync(id, cancellationToken);
        if (employee is null)
        {
            return AppError.NotFound("Employee.NotFound", "Employee not found.");
        }

        var email = EmailAddress.Create(input.Email);
        var mobileNumber = MobileNumber.Create(input.MobileNumber);
        var conflicts = await FindConflictsAsync(email, mobileNumber, id, cancellationToken);

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

        await employeeRepository.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    private async Task<List<AppError>> FindConflictsAsync(
        EmailAddress email,
        MobileNumber mobileNumber,
        Guid? excludingEmployeeId,
        CancellationToken cancellationToken)
    {
        List<AppError> errors = [];

        if (await employeeRepository.EmailExistsAsync(
                email, excludingEmployeeId, cancellationToken))
        {
            errors.Add(AppError.Conflict("Employee.Email.Exists", "Email is already in use."));
        }

        if (await employeeRepository.MobileNumberExistsAsync(
                mobileNumber, excludingEmployeeId, cancellationToken))
        {
            errors.Add(AppError.Conflict(
                "Employee.MobileNumber.Exists",
                "Mobile number is already in use."));
        }

        return errors;
    }
}
