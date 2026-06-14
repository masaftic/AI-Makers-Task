using EmployeeManagement.Domain.EmployeeRoot;

namespace EmployeeManagement.Infrastructure.Persistence.Seeds;

internal static class EmployeeSeed
{
    private const int EngineeringDepartmentId = 1;

    private const int PeopleDepartmentId = 2;

    public static readonly Employee[] All =
    [
        Employee.Create(
            1,
            "Mona Hassan",
            EmailAddress.Create("mona.hassan@example.com"),
            MobileNumber.Create("+201001112233"),
            EngineeringDepartmentId,
            "Senior Software Engineer",
            new DateOnly(2022, 3, 14),
            true),
        Employee.Create(
            2,
            "Omar Khalil",
            EmailAddress.Create("omar.khalil@example.com"),
            MobileNumber.Create("+201002223344"),
            EngineeringDepartmentId,
            "Product Engineer",
            new DateOnly(2023, 7, 2),
            true),
        Employee.Create(
            3,
            "Nour Adel",
            EmailAddress.Create("nour.adel@example.com"),
            MobileNumber.Create("+201003334455"),
            PeopleDepartmentId,
            "People Operations Manager",
            new DateOnly(2021, 11, 8),
            false)
    ];
}
