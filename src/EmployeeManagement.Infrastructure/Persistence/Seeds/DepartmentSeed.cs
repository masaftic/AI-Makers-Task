using EmployeeManagement.Domain.DepartmentRoot;

namespace EmployeeManagement.Infrastructure.Persistence.Seeds;

internal static class DepartmentSeed
{
    public static readonly Department[] All =
    [
        Department.Create(
            1,
            "Engineering"),
        Department.Create(
            2,
            "People")
    ];
}
