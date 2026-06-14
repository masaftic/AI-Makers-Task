using EmployeeManagement.Domain.DepartmentRoot;

namespace EmployeeManagement.Infrastructure.Persistence;

internal static class DepartmentSeed
{
    public static readonly Department[] All =
    [
        Department.Create(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "Engineering"),
        Department.Create(
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "People")
    ];
}
