using EmployeeManagement.Domain.EmployeeRoot;

namespace EmployeeManagement.Application.Employees;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<EmployeeProjection>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<EmployeeProjection?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Employee?> GetForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);

    void Remove(Employee employee);

    Task<bool> EmailExistsAsync(
        EmailAddress email,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> MobileNumberExistsAsync(
        MobileNumber mobileNumber,
        Guid? excludingEmployeeId = null,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
