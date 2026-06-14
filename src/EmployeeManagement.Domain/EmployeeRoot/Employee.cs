using Ardalis.GuardClauses;

namespace EmployeeManagement.Domain.EmployeeRoot;


public class Employee
{
    public Guid Id { get; init; }
    public string FullName { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = null!;
    public MobileNumber MobileNumber { get; private set; } = null!;
    public Guid DepartmentId { get; private set; }
    public string JobTitle { get; private set; } = null!;
    public DateOnly HireDate { get; private set; }
    public bool IsActive { get; private set; }

    private Employee() { }

    private Employee(Guid id, string fullName, EmailAddress email, MobileNumber mobileNumber, Guid departmentId, string jobTitle, DateOnly hireDate, bool isActive)
    {
        Guard.Against.NullOrEmpty(fullName, nameof(fullName));
        Guard.Against.NullOrEmpty(jobTitle, nameof(jobTitle));

        Id = id;
        FullName = fullName;
        Email = email;
        MobileNumber = mobileNumber;
        DepartmentId = departmentId;
        JobTitle = jobTitle;
        HireDate = hireDate;
        IsActive = isActive;
    }

    public static Employee Create(
        Guid id,
        string fullName,
        EmailAddress email,
        MobileNumber mobileNumber,
        Guid departmentId,
        string jobTitle,
        DateOnly hireDate,
        bool isActive)
    {
        return new(id, fullName, email, mobileNumber, departmentId, jobTitle, hireDate, isActive);
    }

    public void Update(
        string fullName,
        EmailAddress email,
        MobileNumber mobileNumber,
        Guid departmentId,
        string jobTitle,
        DateOnly hireDate,
        bool isActive)
    {
        Guard.Against.NullOrEmpty(fullName, nameof(fullName));
        Guard.Against.NullOrEmpty(jobTitle, nameof(jobTitle));

        FullName = fullName;
        Email = email;
        MobileNumber = mobileNumber;
        DepartmentId = departmentId;
        JobTitle = jobTitle;
        HireDate = hireDate;
        IsActive = isActive;
    }
}
