using EmployeeManagement.Domain.EmployeeRoot;

namespace EmployeeManagement.Infrastructure.Persistence;

internal static class EmployeeSeed
{
    private static readonly Guid EngineeringDepartmentId =
        Guid.Parse("10000000-0000-0000-0000-000000000001");

    private static readonly Guid PeopleDepartmentId =
        Guid.Parse("10000000-0000-0000-0000-000000000002");

    public static readonly Employee[] All =
    [
        Employee.Create(
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            "Mona Hassan",
            EmailAddress.Create("mona.hassan@example.com"),
            MobileNumber.Create("+201001112233"),
            EngineeringDepartmentId,
            "Senior Software Engineer",
            new DateOnly(2022, 3, 14),
            true),
        Employee.Create(
            Guid.Parse("20000000-0000-0000-0000-000000000002"),
            "Omar Khalil",
            EmailAddress.Create("omar.khalil@example.com"),
            MobileNumber.Create("+201002223344"),
            EngineeringDepartmentId,
            "Product Engineer",
            new DateOnly(2023, 7, 2),
            true),
        Employee.Create(
            Guid.Parse("20000000-0000-0000-0000-000000000003"),
            "Nour Adel",
            EmailAddress.Create("nour.adel@example.com"),
            MobileNumber.Create("+201003334455"),
            PeopleDepartmentId,
            "People Operations Manager",
            new DateOnly(2021, 11, 8),
            false)
    ];
}
